using Kastra.Core.DTO;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business.Mappers
{
    public static class FileMapper
	{
		/// <summary>
		/// Convert File to File info.
		/// </summary>
		/// <param name="file">File</param>
		/// <returns></returns>
        public static FileInfo ToFileInfo(this File file)
		{
			if (file is null)
            {
				return null;
            }
 
			return new FileInfo()
			{
				FileId = file.FileId,
				Name = file.Name,
				Path = file.Path
			};
		}

		/// <summary>
		/// Convert FileInfo to File.
		/// </summary>
		/// <param name="fileInfo">File info</param>
		/// <returns>File</returns>
		public static File ToFile(this FileInfo fileInfo)
		{
			if (fileInfo is null)
            {
				return null;
            }

			return new File()
			{
				FileId = fileInfo.FileId,
				Name = fileInfo.Name,
				Path = fileInfo.Path
			};
		}
	}
}
