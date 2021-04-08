using ReaderApp.Services.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ReaderApp.Services.Concrete
{
    public class LocalUserDictionary : IUserDictionary
    {
        private readonly string _localDictionariesPath;

        public LocalUserDictionary(string localDictionariesPath) => _localDictionariesPath = localDictionariesPath;

        public async Task<IEnumerable<string>> GetLearned(Guid userId)
        {
            var filePath = GetDictionaryPath(userId);
            if (!File.Exists(filePath))
                throw new Exception($"User dictionary doesn`t exist when requested read. User id: {userId}");

            return await File.ReadAllLinesAsync(filePath, Encoding.UTF8);
        }

        public async Task<IEnumerable<string>> SelectUnknownWords(Guid userId, IEnumerable<string> words)
        {
            var learnedWords = new HashSet<string>(await GetLearned(userId));
            var unknownWords = new List<string>();
            foreach (var word in words)
            {
                if (!learnedWords.Contains(word))
                    unknownWords.Add(word);
            }

            return unknownWords;
        }

        public async Task AddToLearned(Guid userId, IEnumerable<string> learnedWords)
        {
            var dictionary = new HashSet<string>(await GetLearned(userId));
            dictionary.UnionWith(learnedWords);
            await File.WriteAllLinesAsync(GetDictionaryPath(userId), dictionary, Encoding.UTF8);
        }

        public void Create(Guid userId)
        {
            var filePath = GetDictionaryPath(userId);
            if (File.Exists(filePath))
                throw new Exception($"Trying to create an already existing dictionary. User id: {userId}");

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            File.Create(filePath).Close();
        }

        private string GetDictionaryPath(Guid userId) => Path.Combine(_localDictionariesPath, userId.ToString(), "Dictionary", "English", "dictionary.txt");
    }
}
