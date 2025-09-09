using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.Exceptions;

namespace LMS.Services
{
    public class FileHandlerService : IFileHandlerService
    {
        private readonly string _rootPath;
        public FileHandlerService(string rootPath)
        {
            _rootPath = rootPath;
        }
        public async Task<bool> DeleteFileAsync(string fileUrl)
        {
            var filePath = Path.Combine(_rootPath, fileUrl);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<(Stream Stream, string ContentType, string FileName)?> GetFileByPathAsync(string path)
        {
            var fullPath = Path.Combine(_rootPath, path);
            if (!File.Exists(fullPath))
                return null;

            var extension = Path.GetExtension(fullPath).ToLowerInvariant();
            var contentType = extension switch
            {
                ".pdf" => "application/pdf",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };

            var fileName = Path.GetFileName(fullPath);
            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return (stream, contentType, fileName);
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder)
        {
            //I do not know if this is correct
            try
            {
                var folderPath = Path.Combine(_rootPath, folder);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var filePath = Path.Combine(folderPath, fileName);
                using (var file = new FileStream(filePath, FileMode.Create))
                {
                    await fileStream.CopyToAsync(file);
                }
                return Path.GetRelativePath(_rootPath, filePath).Replace("\\", "/");
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                throw new Exception("An error occurred while uploading the file.", ex);
            }
        }
    }
}
