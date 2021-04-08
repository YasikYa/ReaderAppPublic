using ReaderApp.Services.Abstract;
using System.Threading.Tasks;

namespace ReaderApp.Services.Concrete
{
    public class LemmaSharpAdapter : ILemmatizer
    {
        private LemmaSharp.ILemmatizer _innerLematizer;

        public LemmaSharpAdapter(LemmaSharp.ILemmatizer lemmatizer) => _innerLematizer = lemmatizer;

        public Task<string> Lemmatize(string word)
        {
            return Task.FromResult(_innerLematizer.Lemmatize(word));
        }
    }
}
