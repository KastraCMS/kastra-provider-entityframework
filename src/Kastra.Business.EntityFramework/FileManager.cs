using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Kastra.Business.Mappers;
using Kastra.Core;
using Kastra.Core.Business;
using Kastra.Core.Dto;
using Kastra.DAL.EntityFramework;
using Kastra.DAL.EntityFramework.Models;
using Microsoft.Extensions.Configuration;

namespace Kastra.Business
{
    public class FileManager : IFileManager
    {
        private KastraContext _dbContext = null;
        private readonly AppSettings _appSettings = null;

        public FileManager(KastraContext dbContext, IParameterManager parameterManager, IConfiguration configuration)
        {
            _appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
            _dbContext = dbContext;
        }

        public void AddFile(Core.Dto.FileInfo file, Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (file.FileId != null && _dbContext.KastraFiles.Any(f => f.FileId == file.FileId))
            {
                throw new ArgumentException($"{nameof(file.FileId)} already exists");
            }

            // Generate new file id
            file.FileId = Guid.NewGuid();

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
            }
            catch (Exception e)
            {
                File.Delete(filePath);
                throw e;
            }
        }

        public void DeleteFile(Guid fileId)
        {
            if (fileId == null)
            {
                throw new ArgumentNullException(nameof(fileId));
            }
            
            KastraFiles file = _dbContext.KastraFiles.SingleOrDefault(f => f.FileId == fileId);

            if (file == null)
            {
                throw new Exception($"{nameof(fileId)} not found");
            }

            _dbContext.KastraFiles.Remove(file);
        }

        public void DownloadFileByGuid(Guid fileId)
        {
            if (fileId == null)
            {
                throw new ArgumentNullException(nameof(fileId));
            }

            Core.Dto.FileInfo file = _dbContext.KastraFiles.SingleOrDefault(f => f.FileId == fileId)?.ToFileInfo();

            if (file == null)
            {
                throw new Exception($"{nameof(fileId)} not found");
            }

            //return File.ReadAllBytes(GetFilePath(file);
        }

        public Core.Dto.FileInfo GetFile(Guid fileId)
        {
            if (fileId == null)
            {
                throw new ArgumentNullException(nameof(fileId));
            }

            KastraFiles file = _dbContext.KastraFiles.SingleOrDefault(f => f.FileId == fileId);

            return file?.ToFileInfo();
        }

        public IList<Core.Dto.FileInfo> GetFilesByPath(string path)
        {
            if (String.IsNullOrEmpty(path))
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
            sb.Append($"{file.FileId}_{file.Name}");

            return sb.ToString();
        }

        #endregion
    }
}