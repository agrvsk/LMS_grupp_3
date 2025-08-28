using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IFileHandlerService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder);
        Task<bool> DeleteFileAsync(string fileUrl);
    }
}
