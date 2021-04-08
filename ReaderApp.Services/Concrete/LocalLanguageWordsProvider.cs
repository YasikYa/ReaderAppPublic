using ReaderApp.Services.Abstract;
using System.IO;
using System.Threading.Tasks;

namespace ReaderApp.Services.Concrete
{
    public class LocalLanguageWordsProvider : ILanguageWordsProvider
    {
        private readonly string _languageWordsFilesPath;

        public LocalLanguageWordsProvider(string languageWordsFilesPath) => _languageWordsFilesPath = languageWordsFilesPath;

        public Task<string[]> GetLanguageBasicValidationStopwords() => File.ReadAllLinesAsync(GetStopwordsPath(null));

        public Task<string[]> GetLanguageLexicalWords() => File.ReadAllLinesAsync(GetAllWordsPath(null));

        private string GetAllWordsPath(string language) => Path.Combine(GetBasePath(language), "words.txt");

        private string GetStopwordsPath(string language) => Path.Combine(GetBasePath(language), "stopwords.txt");

        private string GetBasePath(string language) => Path.Combine(_languageWordsFilesPath, "English"); // TODO: support for multiply languages could be added
    }
}
