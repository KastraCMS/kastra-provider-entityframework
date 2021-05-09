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

namespace Kastra.Business
{
    public class FileManager : IFileManager
    {
        private KastraDbContext _dbContext;
        private readonly AppSettings _appSettings;

        public FileManager(KastraDbContext dbContext, IConfiguration configuration)
        {
            _appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
            _dbContext = dbContext;
        }

        public void AddFile(Dto.FileInfo file, Stream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (file.FileId != Guid.Empty && _dbContext.KastraFiles.Any(f => f.FileId == file.FileId))
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
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                File.Delete(filePath);

                throw;
            }
        }

        public void DeleteFile(Guid fileId)
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
            _dbContext.SaveChanges();
        }

        public byte[] DownloadFileByGuid(Guid fileId)
        {
            Dto.FileInfo file = _dbContext.KastraFiles
                .SingleOrDefault(f => f.FileId == fileId)
                .ToFileInfo();

            if (file is null)
            {
                throw new Exception($"{nameof(fileId)} not found");
            }

            return File.ReadAllBytes(GetFilePath(file));
        }

        public Dto.FileInfo GetFile(Guid fileId)
        {
            return _dbContext.KastraFiles
                .SingleOrDefault(f => f.FileId == fileId)
                .ToFileInfo();
        }

        public IList<Core.DTO.FileInfo> GetFilesByPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            IList<Dto.FileInfo> filesInfo = null;
            List<Models.File> files = _dbContext.KastraFiles
                .Where(f => f.Path == path)
                .ToList();
            
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

        #region Private methods

        /// <summary>
        /// Get the file path of file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        protected string GetFilePath(Dto.FileInfo file)
        {
            StringBuilder sb = new StringBuilder();
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