using ReaderApp.Services.Abstract;
using System.Collections.Generic;

namespace ReaderApp.Services.Concrete.WordsFilters
{
    public class LemmaFilter : WordPipeFilterBase
    {
        private readonly ILemmatizer _lemmatizer;

        public HashSet<string> Lemmas { get; } = new HashSet<string>();

        public LemmaFilter(ILemmatizer lemmatizer) => _lemmatizer = lemmatizer;

        public override IEnumerable<string> Pipe(IEnumerable<string> incoming)
        {
            foreach (var word in base.Pipe(incoming))
            {
                var lemma = _lemmatizer.Lemmatize(word).Result; // TODO: Redesign to use async call in pipe
                if (Lemmas.Contains(lemma))
                    continue;

                Lemmas.Add(lemma);
                yield return lemma;
            }
        }
    }
}
