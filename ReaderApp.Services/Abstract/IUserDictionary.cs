using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReaderApp.Services.Abstract
{
    public interface IUserDictionary
    {
        Task<IEnumerable<string>> GetLearned(Guid userId);

        Task<IEnumerable<string>> SelectUnknownWords(Guid userId, IEnumerable<string> words);

        Task AddToLearned(Guid userId, IEnumerable<string> learnedWords);

        void Create(Guid userId);
    }
}
