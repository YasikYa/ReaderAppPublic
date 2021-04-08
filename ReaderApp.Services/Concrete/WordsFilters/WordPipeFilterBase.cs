using ReaderApp.Services.Abstract;
using System.Collections.Generic;

namespace ReaderApp.Services.Concrete.WordsFilters
{
    public abstract class WordPipeFilterBase : IWordsPipeWorker
    {
        protected IWordsPipeWorker _next { get; set; }

        public WordPipeFilterBase SetNext(WordPipeFilterBase next)
        {
            next._next = this;
            return next;
        }

        public virtual IEnumerable<string> Pipe(IEnumerable<string> incoming) => _next != null ? _next.Pipe(incoming) : incoming;
    }
}
