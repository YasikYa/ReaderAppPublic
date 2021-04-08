using System.Collections.Generic;

namespace ReaderApp.Services.Concrete.WordsFilters
{
    public class NotInFilter : WordPipeFilterBase
    {
        private readonly HashSet<string> _filterBy;

        public NotInFilter(HashSet<string> filterBy) => _filterBy = filterBy;

        public override IEnumerable<string> Pipe(IEnumerable<string> incoming)
        {
            foreach (var word in base.Pipe(incoming))
            {
                if (_filterBy.Contains(word))
                    continue;
                else
                    yield return word;
            }
        }
    }
}
