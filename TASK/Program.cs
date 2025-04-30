using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("Введите строку:");
        string input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Ошибка: Строка не может быть пустой!!!");
            return;
        }

        List<char> mistake = new List<char>();

        foreach (char c in input)
        {
            // Проверка с использованием ASCII-кодов
            if (c < 97 || c > 122) // 97 - 'a', 122 - 'z'
            {
                mistake.Add(c);
            }
        }

        if (mistake.Count > 0)
        {
            if (mistake.Count == 1)
            {
                Console.WriteLine("Ошибка: Введен неподходящий символ: " + string.Join(", ", mistake));
            }
            else
            {
                Console.WriteLine("Ошибка: Введены неподходящие символы: " + string.Join(", ", mistake));
            }
            return;
        }

        int length = input.Length;
        string result;
        if (length % 2 == 0)
        {
            int mid = length / 2;
            string firstPart = input.Substring(0, mid);
            string secondPart = input.Substring(mid);

            firstPart = ReverseString(firstPart);
            secondPart = ReverseString(secondPart);

            result = firstPart + secondPart;
        }

        else
        {
            string reversedInput = ReverseString(input);
            result = reversedInput + input;
        }
        Console.WriteLine("Результат: " + result);
        // Подсчет частоты символов в результирующей строке
        Dictionary<char, int> characterCount = CountCharacterFrequency(result);
        Console.WriteLine("Частота символов в обработанной строке:");
        foreach (var kvp in characterCount)
        {
            Console.WriteLine($"'{kvp.Key}': {kvp.Value} раз(а)");
        }
        Console.ReadKey();
    }

    // Метод для переворачивания строки
    static string ReverseString(string s)
    {
        char[] collect = s.ToCharArray();
        Array.Reverse(collect);
        return new string(collect);
    }
     // Метод для подсчета частоты символов
    static Dictionary<char, int> CountCharacterFrequency(string s)
    {
        var frequency = new Dictionary<char, int>();

        foreach (char c in s)
        {
            if (frequency.ContainsKey(c))
            {
                frequency[c]++;
            }
            else
            {
                frequency[c] = 1;
            }
        }

        return frequency;
    }
}

