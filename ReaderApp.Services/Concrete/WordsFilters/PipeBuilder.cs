using ReaderApp.Services.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReaderApp.Services.Concrete.WordsFilters
{
    public class PipeBuilder
    {
        private HashSet<string> _stopwords;
        private HashSet<string> _lexicalWords;

        private readonly ILanguageWordsProvider _languageWordsProvider;

        public PipeBuilder(
            ILanguageWordsProvider languageWordsProvider) => (_languageWordsProvider) = (languageWordsProvider);

        public async Task<IWordsPipeWorker> BuildCompressPipe()
        {
            var stopwords = await GetStopwords();
            var allWords = await GetLanguageWords();
            return new TrimPipe()
                .SetNext(new UniqueWordsFilter())
                .SetNext(new NotInFilter(stopwords))
                .SetNext(new InFilter(allWords));
        }

        private async Task<HashSet<string>> GetStopwords()
        {
            if (_stopwords != null)
                return _stopwords;

            _stopwords = new HashSet<string>(await _languageWordsProvider.GetLanguageBasicValidationStopwords());
            return _stopwords;
        }

        private async Task<HashSet<string>> GetLanguageWords()
        {
            if (_lexicalWords != null)
                return _lexicalWords;

            _lexicalWords = new HashSet<string>(await _languageWordsProvider.GetLanguageLexicalWords());
            return _lexicalWords;
        }
    }
}
