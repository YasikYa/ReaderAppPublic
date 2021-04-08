using System;
using System.IO;

namespace ReaderApp.Services.Abstract
{
    public interface IUserFileStore
    {
        Stream CreateWriteStream(Guid userId, Guid fileId);

        Stream CreateReadStream(Guid userId, Guid fileId);

        void DeleteFile(Guid userId, Guid fileId);
    }
}
