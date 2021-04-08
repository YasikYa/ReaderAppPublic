using ReaderApp.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReaderApp.Services.Models
{
    public class LemmaGroup
    {
        private readonly HashSet<string> _words;

        public string Lemma { get; }

        public IEnumerable<string> Words => _words.ToArray();

        public LemmaGroup(string lemma)
        {
            Lemma = lemma;
            _words = new HashSet<string> { lemma };
        }

        public void AddToGroup(string word) => _words.Add(word);

        public override string ToString()
        {
            return $"{Lemma}\t{string.Join(',', _words)}";
        }

        public async static Task<IEnumerable<LemmaGroup>> GroupLemmas(IEnumerable<string> words, ILemmatizer lemmatizer)
        {
            var map = new Dictionary<string, LemmaGroup>();
            foreach (var word in words)
            {
                var lemma = await lemmatizer.Lemmatize(word);
                if (!map.ContainsKey(lemma))
                    map.Add(lemma, new LemmaGroup(lemma));

                map[lemma].AddToGroup(word);
            }

            return map.Values;
        }

        public static LemmaGroup Parse(string value)
        {
            var parts = value.Split('\t');
            if (parts.Length != 2)
                throw new Exception("Unexpected LemmaGroup format. Value should be in format {lemma\\tword,word}");

            var result = new LemmaGroup(parts[0]);
            foreach (var word in parts[1].Split(','))
                result.AddToGroup(word);

            return result;
        }
    }
}
