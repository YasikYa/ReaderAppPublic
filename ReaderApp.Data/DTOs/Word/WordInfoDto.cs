using System.Collections.Generic;

namespace ReaderApp.Data.DTOs.Word
{
    public class WordInfoDto
    {
        public IEnumerable<string> Definitions { get; set; }

        public IEnumerable<string> Translations { get; set; }

        public string Lemma { get; set; }
    }
}
