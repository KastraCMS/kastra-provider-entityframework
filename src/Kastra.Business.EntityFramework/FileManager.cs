using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Kastra.Business.Mappers;
using Kastra.Core.Business;
using Kastra.Core.Configuration;
using Kastra.DAL.EntityFramework;
using Kastra.DAL.EntityFramework.Models;
using Microsoft.Extensions.Configuration;

namespace Kastra.Business
{
    public class FileManager : IFileManager
    {
        private KastraContext _dbContext;
        private readonly AppSettings _appSettings;

        public FileManager(KastraContext dbContext, IParameterManager parameterManager, IConfiguration configuration)
        {
            _appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
            _dbContext = dbContext;
        }

        public void AddFile(Core.Dto.FileInfo file, Stream stream)
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
                using (FileStream fileStream = File.Create(filePath, (int)stream.Length))
                {
                    byte[] bytesInStream = new byte[stream.Length];
                    stream.Read(bytesInStream, 0, bytesInStream.Length);    
                    fileStream.Write(bytesInStream, 0, bytesInStream.Length);
                }

                // Save file in database
                _dbContext.KastraFiles.Add(file.ToKastraFile());
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                File.Delete(filePath);
                throw e;
            }
        }

        public void DeleteFile(Guid fileId)
        {   
            KastraFiles file = _dbContext.KastraFiles.SingleOrDefault(f => f.FileId == fileId);

            if (file is null)
            {
                throw new Exception($"{nameof(fileId)} not found");
            }

            _dbContext.KastraFiles.Remove(file);
        }

        public byte[] DownloadFileByGuid(Guid fileId)
        {
            Core.Dto.FileInfo file = _dbContext.KastraFiles.SingleOrDefault(f => f.FileId == fileId)?.ToFileInfo();

            if (file == null)
            {
                throw new Exception($"{nameof(fileId)} not found");
            }

            return File.ReadAllBytes(GetFilePath(file));
        }

        public Core.Dto.FileInfo GetFile(Guid fileId)
        {
            KastraFiles file = _dbContext.KastraFiles.SingleOrDefault(f => f.FileId == fileId);

            return file?.ToFileInfo();
        }

        public IList<Core.Dto.FileInfo> GetFilesByPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            IList<Core.Dto.FileInfo> filesInfo = null;
            List<KastraFiles> files = _dbContext.KastraFiles.Where(f => f.Path == path).ToList();
            
            if (files == null)
            {
                return null;
            }

            filesInfo = new List<Core.Dto.FileInfo>(files.Count);

            foreach (KastraFiles file in files)
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
        protected string GetFilePath(Core.Dto.FileInfo file)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_appSettings.Configuration.FileDirectoryPath.TrimEnd(Path.DirectorySeparatorChar));
            sb.Append(Path.DirectorySeparatorChar);
            sb.Append(file.Path.Replace("..", String.Empty));
            sb.Append(Path.DirectorySeparatorChar);
            sb.Append(Path.GetFileName(file.Name));

            return sb.ToString();
        }

        #endregion
    }
}