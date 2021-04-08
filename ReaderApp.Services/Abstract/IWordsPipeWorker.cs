using System.Collections.Generic;

namespace ReaderApp.Services.Abstract
{
    public interface IWordsPipeWorker
    {
        IEnumerable<string> Pipe(IEnumerable<string> incoming);
    }
}
