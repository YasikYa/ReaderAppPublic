using ReaderApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReaderApp.Services.Abstract
{
    public interface IFileProcessedWordsCache
    {
        Task CacheCompressedAsync(Guid userId, Guid fileId, IEnumerable<LemmaGroup> compressedWords);

        Task<IEnumerable<LemmaGroup>> ExtractCompressedAsync(Guid userId, Guid fileId);

        void ClearCache(Guid userId, Guid fileId);
    }
}
