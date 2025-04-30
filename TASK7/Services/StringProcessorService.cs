using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TASK7.Models.Config;
using StringProcessor.API.Models.Requests;
using StringProcessor.API.Models.Responses;
using StringProcessor.API.Services.Interfaces;
using StringProcessor.API.Utilities;

namespace StringProcessor.API.Services
{
    public class StringProcessorService : IStringProcessorService
    {
        private readonly IRandomNumberGenerator _randomNumberGenerator;
        private readonly RandomApiConfig _config;
        private const string Vowels = "aeiouy";

        public StringProcessorService(
            IRandomNumberGenerator randomNumberGenerator,
            IOptions<RandomApiConfig> config)
        {
            _randomNumberGenerator = randomNumberGenerator;
            _config = config.Value;
        }

        public async Task<ProcessStringResponse> ProcessString(ProcessStringRequest request)
        {
            ValidateInput(request.Input);

            var processedString = ProcessStringInternal(request.Input);
            var randomPosition = await _randomNumberGenerator.GetRandomPosition(processedString.Length);

            return new ProcessStringResponse
            {
                ProcessedString = processedString,
                CharacterFrequency = CountCharacterFrequency(processedString),
                LongestVowelSubstring = GetLongestSubstringWithVowels(processedString),
                SortedString = SortString(processedString, request.SortMethod),
                TrimmedString = processedString.Remove(randomPosition, 1),
                RemovedCharPosition = randomPosition
            };
        }

        private void ValidateInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Строка не может быть пустой!");

            if (_config.Settings.BlackList?.Contains(input.ToLower()) == true)
                throw new ArgumentException($"Строка '{input}' находится в чёрном списке");

            var invalidChars = input.Where(c => c < 97 || c > 122).ToList();
            if (invalidChars.Any())
                throw new ArgumentException($"Недопустимые символы: {string.Join(", ", invalidChars)}");
        }

        private string ProcessStringInternal(string input)
        {
            int length = input.Length;

            if (length % 2 == 0)
            {
                int mid = length / 2;
                string firstPart = input.Substring(0, mid);
                string secondPart = input.Substring(mid);

                return ReverseString(firstPart) + ReverseString(secondPart);
            }
            else
            {
                string reversedInput = ReverseString(input);
                return reversedInput + input;
            }
        }

        private string ReverseString(string s)
        {
            char[] array = s.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }

        private Dictionary<char, int> CountCharacterFrequency(string s)
        {
            var frequency = new Dictionary<char, int>();
            foreach (char c in s)
            {
                frequency[c] = frequency.ContainsKey(c) ? frequency[c] + 1 : 1;
            }
            return frequency;
        }

        private string GetLongestSubstringWithVowels(string s)
        {
            string longest = string.Empty;
            for (int i = 0; i < s.Length; i++)
            {
                if (Vowels.Contains(s[i]))
                {
                    for (int j = i + 1; j < s.Length; j++)
                    {
                        if (Vowels.Contains(s[j]))
                        {
                            string current = s.Substring(i, j - i + 1);
                            if (current.Length > longest.Length)
                            {
                                longest = current;
                            }
                        }
                    }
                }
            }
            return longest;
        }

        private string SortString(string input, int sortMethod)
        {
            return sortMethod switch
            {
                1 => $"Быстрая сортировка: {QuickSort(input)}",
                2 => $"Сортировка деревом: {TreeSort(input)}",
                3 => $"Быстрая сортировка: {QuickSort(input)}\nСортировка деревом: {TreeSort(input)}",
                _ => "Неизвестный метод сортировки"
            };
        }

        private string QuickSort(string input)
        {
            char[] arr = input.ToCharArray();
            QuickSort(arr, 0, arr.Length - 1);
            return new string(arr);
        }

        private void QuickSort(char[] arr, int low, int high)
        {
            if (low < high)
            {
                int pi = Partition(arr, low, high);
                QuickSort(arr, low, pi - 1);
                QuickSort(arr, pi + 1, high);
            }
        }

        private int Partition(char[] arr, int low, int high)
        {
            char pivot = arr[high];
            int i = low - 1;
            for (int j = low; j < high; j++)
            {
                if (arr[j] < pivot)
                {
                    i++;
                    Swap(ref arr[i], ref arr[j]);
                }
            }
            Swap(ref arr[i + 1], ref arr[high]);
            return i + 1;
        }

        private void Swap(ref char a, ref char b) => (a, b) = (b, a);

        private string TreeSort(string input)
        {
            var root = BuildTree(input);
            var sorted = new List<char>();
            InOrderTraversal(root, sorted);
            return new string(sorted.ToArray());
        }

        private Node BuildTree(string input)
        {
            Node root = null;
            foreach (char c in input)
            {
                root = InsertNode(root, c);
            }
            return root;
        }

        private Node InsertNode(Node root, char data)
        {
            if (root == null) return new Node(data);
            if (data < root.Data)
                root.Left = InsertNode(root.Left, data);
            else
                root.Right = InsertNode(root.Right, data);
            return root;
        }

        private void InOrderTraversal(Node node, List<char> result)
        {
            if (node != null)
            {
                InOrderTraversal(node.Left, result);
                result.Add(node.Data);
                InOrderTraversal(node.Right, result);
            }
        }

        private class Node
        {
            public char Data { get; }
            public Node Left { get; set; }
            public Node Right { get; set; }

            public Node(char data) => Data = data;
        }
    }
}