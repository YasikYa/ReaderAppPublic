using System.Collections.Generic;
using System.Linq;

namespace ReaderApp.Services.Concrete.WordsFilters
{
    public class TrimPipe : WordPipeFilterBase
    {
        public override IEnumerable<string> Pipe(IEnumerable<string> incoming) => base.Pipe(incoming).Select(word => word.Trim(new[] { '!', ',', '.', '?', '\\', ';', '/', '(', ')', ':', '\'', '\"', '\r' }).ToLower());
    }
}
