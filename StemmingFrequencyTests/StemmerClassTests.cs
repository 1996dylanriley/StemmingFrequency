using StemmingFrequency;
using System;
using System.Collections.Generic;
using Xunit;

namespace StemmingFrequencyTests
{
    public class StemmerClassTests
    {
        [Fact]
        public void CanCountSingleStemWord()
        {
            var input = "Friend";
            var wordFrequency = new Stemmer(input).Frequency("Friend");
            Assert.Equal(1, wordFrequency);
        }

        [Fact]
        public void CanCountDuplicateRootWords()
        {
            var input = "Friend friend";
            var wordFrequency = new Stemmer(input).Frequency("friend");
            Assert.Equal(2, wordFrequency);
        }

        [Fact]
        public void CanCountSuffixedWord()
        {
            var input = "Friends Friend";
            var wordFrequency = new Stemmer(input).Frequency("friend");
            Assert.Equal(2, wordFrequency);
        }

        [Theory]
        [InlineData("Friends", "friend")]
        [InlineData("friendlier", "friend")]
        [InlineData("friendlies", "friend")]
        [InlineData("friendly", "friend")]
        [InlineData("classify", "class")]
        [InlineData("classification", "class")]
        [InlineData("class", "class")]
        [InlineData("class", "classes")]
        [InlineData("Flowery", "flower")]
        [InlineData("flowers", "flower")]
        [InlineData("flow" , "flow")]
        [InlineData("following", "follow")]
        public void CanCorrectlyProcessSuffixes(string input, string word)
        {
            var wordFrequency = new Stemmer(input).Frequency(word);
            Assert.Equal(1, wordFrequency);
        }

        [Fact]
        public void CanCountRootFromSuffixedWord()
        {
            var input = "Friend";
            var wordFrequency = new Stemmer(input).Frequency("friendly");
            Assert.Equal(1, wordFrequency);
        }

        [Fact]
        public void CanCountRootWordFromSuffixedWordsGivenSuffixedWord()
        {
            var input = "classes classes";
            var wordFrequency = new Stemmer(input).Frequency("classification");
            Assert.Equal(2, wordFrequency);
        }

        [Theory]
        [InlineData("following", 1)]
        [InlineData("flow", 2)]
        [InlineData("classification", 3)]
        [InlineData("class", 3)]
        [InlineData("flower", 3)]
        [InlineData("friend", 5)]
        [InlineData("friendly", 5)]
        [InlineData("classes", 3)]
        public void CanCountRootWordsInComplexSentence(string word, int expectedCount)
        {
            var input = "Friends are friendlier friendlies that are friendly and classify the friendly classification class. Flowery flowers flow through following the flower flows.";
            var wordFrequency = new Stemmer(input).Frequency(word);
            Assert.Equal(expectedCount, wordFrequency);

        }
    }
}
