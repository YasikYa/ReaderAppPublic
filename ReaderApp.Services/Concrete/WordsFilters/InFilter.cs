using System.Collections.Generic;

namespace ReaderApp.Services.Concrete.WordsFilters
{
    public class InFilter : WordPipeFilterBase
    {
        private readonly HashSet<string> _filterBy;

        public InFilter(HashSet<string> filterBy) => _filterBy = filterBy;

        public override IEnumerable<string> Pipe(IEnumerable<string> incoming)
        {
            foreach (var word in base.Pipe(incoming))
            {
                if (_filterBy.Contains(word))
                    yield return word;
                else
                    continue;
            }
        }
    }
}
