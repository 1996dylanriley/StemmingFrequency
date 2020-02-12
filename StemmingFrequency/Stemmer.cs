using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace StemmingFrequency
{
    public class Stemmer
    {
        private string _term;
        public string Term
        {
            get => _term;
            set => _term = Regex.Replace(value, "[^a-zA-Z0-9 ]+", "", RegexOptions.Compiled).ToLower(); 
        }

        private List<Suffix> Suffixes { get; }

        private List<String> SuffixesByLength =>
            Suffixes.Select(x => x.Value).OrderByDescending(x => x.Length).ToList();
        

        public Stemmer(string term)
        {
            Term = term;
            Suffixes = JsonConvert.DeserializeObject<List<Suffix>>(File.ReadAllText("./Suffixes.json"));
        }

        public int Frequency(string word)
        {
            word = word.ToLower();
            var termWords = Term.Split(' ');
            var result = 0;

            foreach (var tw in termWords)
                 if (tw == word || GetStem(word) == GetStem(tw))
                    result++;

            return result;
        }
           
        private string GetStem(string word)
        {
            var rootWord = word;                 
            while (ContainsAnySuffix(rootWord))
            {
                var suffix = GetSuffix(rootWord);
                var suffixPredecessor = Suffixes.Find(x => x.Value == suffix).Predecessor;
                rootWord = rootWord.Replace(suffix, suffixPredecessor);
            }

            return rootWord;
        }

        private bool ContainsSpecificSuffix(string word, string suffix)
        {
            if (suffix.Length > word.Length)
                return false;

            var suffixIncompatibleWithWord =
               Suffixes.FirstOrDefault(x => x.Value == suffix).IncompatibleWith == word.Substring(word.Length - suffix.Length);

            if (suffixIncompatibleWithWord)
                return false;

            return word.Substring(word.Length - suffix.Length).Contains(suffix);
        }

        private bool ContainsAnySuffix(string word) =>
            SuffixesByLength.Any(x => ContainsSpecificSuffix(word, x));

        private string GetSuffix(string word) =>
            SuffixesByLength.Find(x => ContainsSpecificSuffix(word, x));
    }
}
