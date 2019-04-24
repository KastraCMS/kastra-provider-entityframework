using System;
using System.Linq;
using Kastra.Core;
using Kastra.Core.Dto;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business.Mappers
{
	public static class FileMapper
	{
        public static FileInfo ToFileInfo(this KastraFiles file)
		{
            FileInfo fileInfo = new FileInfo();
			fileInfo.FileId = file.FileId;
			fileInfo.Name = file.Name;
            fileInfo.Path = file.Path;

			return fileInfo;
		}

		public static KastraFiles ToKastraFile(this FileInfo fileInfo)
		{
			KastraFiles file = new KastraFiles();
			file.FileId = fileInfo.FileId;
            file.Name = fileInfo.Name;
            file.Path = fileInfo.Path;

			return file;
		}
	}
}
