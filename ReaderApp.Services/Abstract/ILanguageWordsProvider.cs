using System.Threading.Tasks;

namespace ReaderApp.Services.Abstract
{
    public interface ILanguageWordsProvider
    {
        /// <summary>
        /// Get all lexically valid words for a particular language
        /// </summary>
        /// <returns></returns>
        Task<string[]> GetLanguageLexicalWords();

        /// <summary>
        /// Get stopword list for basic validation. These words should be skipped for all texts.
        /// </summary>
        /// <returns></returns>
        Task<string[]> GetLanguageBasicValidationStopwords();
    }
}
