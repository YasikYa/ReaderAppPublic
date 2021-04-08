using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReaderApp.Services.Abstract
{
    public interface IWordDefinitionProvider
    {
        Task<IEnumerable<string>> GetDefintitions(string word);
    }
}
