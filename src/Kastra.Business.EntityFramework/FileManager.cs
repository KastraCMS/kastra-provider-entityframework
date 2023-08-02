using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Kastra.Business.Mappers;
using Kastra.Core.Services.Contracts;
using Kastra.Core.Configuration;
using Dto = Kastra.Core.DTO;
using Kastra.DAL.EntityFramework;
using Models = Kastra.DAL.EntityFramework.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using nClam;
using Microsoft.Extensions.Logging;

namespace Kastra.Business
{
    public class FileManager : IFileManager
    {
        private readonly KastraDbContext _dbContext;
        private readonly AppSettings _appSettings;
        private readonly ILogger<FileManager> _logger;

        public FileManager(KastraDbContext dbContext, IConfiguration configuration, ILogger<FileManager> logger)
        {
            _appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <inheritdoc cref="IFileManager.AddFileAsync(Dto.FileInfo, Stream)"/>
        public async Task AddFileAsync(Dto.FileInfo file, Stream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (file.FileId != Guid.Empty 
                && (await _dbContext.KastraFiles.AnyAsync(f => f.FileId == file.FileId)))
            {
                throw new ArgumentException($"{nameof(file.FileId)} already exists");
            }

            // Generate new file id
            if (file.FileId == Guid.Empty)
            {
                file.FileId = Guid.NewGuid();
            }

            // Get file path
            string filePath = GetFilePath(file);
            
            try
            {
                // Save file
                using (FileStream fileStream = System.IO.File.Create(filePath, (int)stream.Length))
                {
                    byte[] bytesInStream = new byte[stream.Length];
                    stream.Read(bytesInStream, 0, bytesInStream.Length);    
                    fileStream.Write(bytesInStream, 0, bytesInStream.Length);
                }

                // Save file in database
                _dbContext.KastraFiles.Add(file.ToFile());

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                throw;
            }
        }

        /// <inheritdoc cref="IFileManager.DeleteFileAsync(Guid)" />
        public async Task DeleteFileAsync(Guid fileId)
        {
            Models.File file = _dbContext.KastraFiles.SingleOrDefault(f => f.FileId == fileId);

            if (file is null)
            {
                throw new Exception($"{nameof(fileId)} not found");
            }

            // Get file path
            string filePath = GetFilePath(file.ToFileInfo());

            System.IO.File.Delete(filePath);

            _dbContext.KastraFiles.Remove(file);

            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc cref="IFileManager.DownloadFileByGuidAsync(Guid)" />
        public async Task<byte[]> DownloadFileByGuidAsync(Guid fileId)
        {
            Dto.FileInfo file = (await _dbContext.KastraFiles
                .SingleOrDefaultAsync(f => f.FileId == fileId))
                .ToFileInfo();

            if (file is null)
            {
                throw new Exception($"{nameof(fileId)} not found");
            }

            return File.ReadAllBytes(GetFilePath(file));
        }

        /// <inheritdoc cref="IFileManager.GetFileAsync(Guid)" />
        public async Task<Dto.FileInfo> GetFileAsync(Guid fileId)
        {
            return (await _dbContext.KastraFiles
                .SingleOrDefaultAsync(f => f.FileId == fileId))
                .ToFileInfo();
        }

        /// <inheritdoc cref="IFileManager.GetFilesByPathAsync(string)" />
        public async Task<IList<Dto.FileInfo>> GetFilesByPathAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            IList<Dto.FileInfo> filesInfo = null;
            List<Models.File> files = await _dbContext.KastraFiles
                .Where(f => f.Path == path)
                .ToListAsync();
            
            if (files is null)
            {
                return null;
            }

            filesInfo = new List<Dto.FileInfo>(files.Count);

            foreach (Models.File file in files)
            {
                filesInfo.Add(file.ToFileInfo());
            }

            return filesInfo;
        }

        /// <inheritdoc cref="IFileManager.ScanFileAsync(byte[])" />
        public async Task<bool> ScanFileAsync(byte[] fileBytes)
        {
            ClamAV clamAvSettings = _appSettings.ClamAV;

            if (!clamAvSettings.Enable)
            {
                return true;
            }
            else if (!string.IsNullOrEmpty(clamAvSettings.Host))
            {
                var clamClient = new ClamClient(clamAvSettings.Host, clamAvSettings.Port);

                var result = await clamClient.SendAndScanFileAsync(fileBytes);

                if (result.Result == ClamScanResults.Clean)
                {
                    return true;
                }
                else
                {
                    string message = result.Result switch
                    {
                        ClamScanResults.VirusDetected => "Virus detected",
                        ClamScanResults.Error => "Error in the file",
                        ClamScanResults.Unknown => "Unknown file",
                        _ => "No case available"
                    };

                    _logger.LogWarning(message);
                }
            }

            return false;
        }

        #region Private methods

        /// <summary>
        /// Get the file path of file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        protected string GetFilePath(Dto.FileInfo file)
        {
            StringBuilder sb = new ();
            sb.Append(_appSettings.Configuration.FileDirectoryPath.TrimEnd(Path.DirectorySeparatorChar));
            sb.Append(Path.DirectorySeparatorChar);
            sb.Append(file.Path.Replace("..", string.Empty));
            sb.Append(Path.DirectorySeparatorChar);
            sb.Append(Path.GetFileName(file.Name));

            return sb.ToString();
        }

        #endregion
    }
}