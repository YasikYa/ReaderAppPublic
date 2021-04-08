using System.Threading.Tasks;

namespace ReaderApp.Services.Abstract
{
    public interface ILemmatizer
    {
        Task<string> Lemmatize(string word);
    }
}
