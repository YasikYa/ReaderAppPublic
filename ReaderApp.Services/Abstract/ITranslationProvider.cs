using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReaderApp.Services.Abstract
{
    public interface ITranslationProvider
    {
        Task<IEnumerable<string>> GetTranslations(string word);
    }
}
