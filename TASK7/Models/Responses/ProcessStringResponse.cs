using System.Collections.Generic;

namespace StringProcessor.API.Models.Responses
{
    public class ProcessStringResponse
    {
        public string ProcessedString { get; set; }
        public Dictionary<char, int> CharacterFrequency { get; set; }
        public string LongestVowelSubstring { get; set; }
        public string SortedString { get; set; }
        public string TrimmedString { get; set; }
        public int RemovedCharPosition { get; set; }
    }

}