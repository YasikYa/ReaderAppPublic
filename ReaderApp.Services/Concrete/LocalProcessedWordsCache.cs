using ReaderApp.Services.Abstract;
using ReaderApp.Services.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderApp.Services.Concrete
{
    public class LocalProcessedWordsCache : IFileProcessedWordsCache
    {
        private readonly string _localCachePath;

        public LocalProcessedWordsCache(string localCachePath) => _localCachePath = localCachePath;

        public Task CacheCompressedAsync(Guid userId, Guid fileId, IEnumerable<LemmaGroup> compressedWords)
        {
            return File.WriteAllLinesAsync(
                GetFileCachePath(userId, fileId),
                compressedWords.Select(lg => lg.ToString()),
                Encoding.UTF8);
        }

        public void ClearCache(Guid userId, Guid fileId) => File.Delete(GetFileCachePath(userId, fileId));

        public async Task<IEnumerable<LemmaGroup>> ExtractCompressedAsync(Guid userId, Guid fileId)
        {
            var filePath = GetFileCachePath(userId, fileId);
            if (!File.Exists(filePath))
                throw new Exception($"Words cache file not found for file: {fileId}");

            // Cached file stored as lines with structure of: lemma word,word,word\n
            var allGroups = await File.ReadAllLinesAsync(filePath, Encoding.UTF8);
            return allGroups.Select(lg => LemmaGroup.Parse(lg));
        }

        private string GetFileCachePath(Guid userId, Guid fileId) => Path.ChangeExtension(Path.Combine(_localCachePath, userId.ToString(), "Files", fileId.ToString()), "txt");
    }
}
