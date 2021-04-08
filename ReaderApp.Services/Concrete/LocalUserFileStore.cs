using ReaderApp.Services.Abstract;
using System;
using System.IO;

namespace ReaderApp.Services.Concrete
{
    public class LocalUserFileStore : IUserFileStore
    {
        private readonly string _localFilesPath;

        public LocalUserFileStore(string localFilesPath) => _localFilesPath = localFilesPath;

        public Stream CreateReadStream(Guid userId, Guid fileId)
        {
            var filePath = GetFilePath(userId, fileId);
            if (!File.Exists(filePath))
                throw new Exception($"User file with id: {fileId} does`t exists when requested read stream");

            return File.OpenRead(filePath);
        }

        public Stream CreateWriteStream(Guid userId, Guid fileId)
        {
            var filePath = GetFilePath(userId, fileId);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)); // Create directory if not exists, no side effects on existing one.
            return File.Create(filePath);
        }

        public void DeleteFile(Guid userId, Guid fileId) => File.Delete(GetFilePath(userId, fileId));

        private string GetFilePath(Guid userId, Guid fileId) => Path.Combine(_localFilesPath, userId.ToString(), "Files", fileId.ToString());
    }
}
