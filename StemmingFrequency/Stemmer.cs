using System;
using System.Collections.Generic;
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
        //PUT THIS INTO JSON FILE THAT CAN EASILY BE EXTENDED
        private static Dictionary<string, string> SuffixesToRoot => new Dictionary<string, string>()
        {
            { "s", "" },
            { "y", "" },
            { "es", "" },
            {"ly", "" },
            {"ing", "" },
            {"ify", "" },
            {"lier", "y" },
            {"lies", "ly" },
            {"ification", "ify" },
        };

        private static Dictionary<string, string> SuffixesToRootExceptions => new Dictionary<string, string>()
        {
            { "s", "ss" }
        };

        private static List<String> SuffixesByLength =>
            SuffixesToRoot.OrderByDescending(x => x.Key.Length).Select(x => x.Key).ToList();
        

        public Stemmer(string term)
        {
            Term = term;
        }

        public int Frequency(string word)
        {
            word = word.ToLower();
            var termWords = Term.Split(' ');
            var result = 0;

            foreach (var tw in termWords)
            {
                if (word == GetStem(tw))
                    result++;
                else if (tw == GetStem(word))
                    result++;
                else if (tw == word)
                    result++;
                else if(GetStem(word) == GetStem(tw))
                    result++;
            }

            return result;
        }
           
        private string GetStem(string word)
        {
            var rootWord = word;                 
            while (ContainsAnySuffix(rootWord))
            {
                var suffix = GetSuffix(rootWord);
                rootWord = rootWord.Replace(suffix, SuffixesToRoot[suffix]);
            }

            return rootWord;
        }

        private bool ContainsSpecificSuffix(string word, string suffix)
        {
            var wordEndsInSuffixException = SuffixesToRootExceptions.ContainsKey(suffix) &&
                SuffixesToRootExceptions[suffix] == word.Substring(word.Length - SuffixesToRootExceptions[suffix].Length);

            if (wordEndsInSuffixException)
                return false;

            return suffix.Length > word.Length ? false : word.Substring(word.Length - suffix.Length).Contains(suffix);
        }
        private bool ContainsAnySuffix(string word) =>
            SuffixesByLength.Any(x => ContainsSpecificSuffix(word, x));
        private string GetSuffix(string word) =>
            SuffixesByLength.Find(x => ContainsSpecificSuffix(word, x));
    }
}
