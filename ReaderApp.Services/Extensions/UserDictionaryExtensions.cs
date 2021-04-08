using ReaderApp.Services.Abstract;
using ReaderApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReaderApp.Services.Extensions
{
    public static class UserDictionaryExtensions
    {
        public static async Task<IEnumerable<string>> SelectUnknownWordsFromLemmaGroups(this IUserDictionary userDictionary, Guid userId, IEnumerable<LemmaGroup> lemmaGroups)
        {
            var unknownLemmas = new HashSet<string>(await userDictionary.SelectUnknownWords(userId, lemmaGroups.Select(lg => lg.Lemma)));
            return lemmaGroups.Where(lg => unknownLemmas.Contains(lg.Lemma)).SelectMany(lg => lg.Words);
        }
    }
}
