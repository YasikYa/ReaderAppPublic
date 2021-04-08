using System.Collections.Generic;

namespace ReaderApp.Services.Concrete.WordsFilters
{
    public class UniqueWordsFilter : WordPipeFilterBase
    {

        public HashSet<string> UniqueEntries { get; } = new HashSet<string>();

        public override IEnumerable<string> Pipe(IEnumerable<string> incoming)
        {
            foreach (var word in base.Pipe(incoming))
            {
                if (UniqueEntries.Contains(word))
                    continue;

                UniqueEntries.Add(word);
                yield return word;
            }
        }
    }
}
