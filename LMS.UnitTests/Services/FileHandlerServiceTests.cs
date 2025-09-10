using LMS.Services;
using LMS.UnitTests.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.UnitTests.Services
{
    public class FileHandlerServiceTests : ServiceTestBase, IDisposable
    {
        private readonly string _testRootPath;
        private readonly FileHandlerService _service;

        public FileHandlerServiceTests()
        {
            _testRootPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testRootPath);

            _service = new FileHandlerService(_testRootPath);
        }

        [Fact]
        [Trait("FileHandlerService", "UploadFile")]
        public async Task UploadFileAsync_ValidStream_SavesFile()
        {
            var fileName = "test.txt";
            var content = "hello world";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            var relativePath = await _service.UploadFileAsync(stream, fileName, "uploads");

            var fullPath = Path.Combine(_testRootPath, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
            Assert.True(File.Exists(fullPath));

            var savedContent = await File.ReadAllTextAsync(fullPath);
            Assert.Equal(content, savedContent);
        }

        [Fact]
        [Trait("FileHandlerService", "DeleteFile")]
        public async Task DeleteFileAsync_FileExists_DeletesAndReturnsTrue()
        {
            var fileName = "delete.txt";
            var fullPath = Path.Combine(_testRootPath, fileName);
            await File.WriteAllTextAsync(fullPath, "to delete");

            var result = await _service.DeleteFileAsync(fileName);

            Assert.True(result);
            Assert.False(File.Exists(fullPath));
        }

        [Fact]
        [Trait("FileHandlerService", "GetFile")]
        public async Task GetFileByPathAsync_FileExists_ReturnsStreamAndMetadata()
        {
            var fileName = "file.pdf";
            var fullPath = Path.Combine(_testRootPath, fileName);
            await File.WriteAllTextAsync(fullPath, "pdf-content");

            var result = await _service.GetFileByPathAsync(fileName);

            Assert.NotNull(result);
            var (stream, contentType, returnedFileName) = result!.Value;

            Assert.Equal("application/pdf", contentType);
            Assert.Equal(fileName, returnedFileName);

            using var reader = new StreamReader(stream);
            var content = await reader.ReadToEndAsync();
            Assert.Equal("pdf-content", content);
        }

        public void Dispose()
        {
            if (Directory.Exists(_testRootPath))
                Directory.Delete(_testRootPath, recursive: true);
        }
    }
}