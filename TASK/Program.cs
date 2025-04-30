using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static readonly HttpClient client = new HttpClient();
    static readonly Random random = new Random();

    static async Task Main()
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

        // Вывод выбора алгоритма сортировки в самом Main
        Console.WriteLine("Выберите алгоритм сортировки (1 - Быстрая сортировка, 2 - Сортировка деревом, 3 - Оба метода):");
        string choice = Console.ReadLine();

        // Получение случайного индекса через удалённый API
        int randomPosition = await GetRandomPositionAsync(input.Length);
        if (randomPosition == -1)
        {
            // Если не удалось получить случайное число, используем .NET
            randomPosition = random.Next(0, input.Length);
        }

        // Реверсирование строки
        string result = ProcessString(input);
        Console.WriteLine("1. Результат: " + result);

        // Частота символов
        Dictionary<char, int> characterCount = CountCharacterFrequency(result);
        Console.WriteLine("2. Частота символов в обработанной строке:");
        foreach (var kvp in characterCount)
        {
            Console.WriteLine($"'{kvp.Key}': {kvp.Value} раз(а)");
        }

        // Длинная подстрока с гласными
        string getLongestSVowels = GetLongestSubstringWithVowels(result);
        Console.WriteLine("3. Самая длинная подстрока начинающаяся и заканчивающаяся на гласную: " + getLongestSVowels);

        //сортировка
        string resultSort = SortResult(input, choice);
        Console.WriteLine(resultSort);

        // Удаление символа по случайной позиции
        string trimmedResult = result.Remove(randomPosition, 1);
        Console.WriteLine($"5. Урезанная строка (буква на позиции {randomPosition} удалена): " + trimmedResult);
        Console.ReadKey();
    }
    // метод для получения случайного индекса
    static async Task<int> GetRandomPositionAsync(int maxLength)
    {
        try
        {
            // Формируем URL для API
            string url = $"http://www.randomnumberapi.com/api/v1.0/random?min=0&max={maxLength - 1}&count=1";
            var response = await client.GetStringAsync(url);

            // Десериализация ответа
            var randomNumberList = JsonSerializer.Deserialize<List<int>>(response);

            // Проверка, что API вернул значение
            if (randomNumberList != null && randomNumberList.Count > 0)
            {
                return randomNumberList[0]; // Возвращаем первое случайное число
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении случайного числа через API: {ex.Message}");
        }
        // Если нет результата из API, генерируем случайное число
        return random.Next(0, maxLength);
    }


    static string ProcessString(string input)
    {
        int length = input.Length;

        if (length % 2 == 0)
        {
            int mid = length / 2;
            string firstPart = input.Substring(0, mid);
            string secondPart = input.Substring(mid);

            firstPart = ReverseString(firstPart);
            secondPart = ReverseString(secondPart);

            return ReverseString(firstPart) + ReverseString(secondPart);
        }

        else
        {
            string reversedInput = ReverseString(input);
            return reversedInput + input;
        }

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
    // Метод для нахождения наибольшей подстроки, начинающейся и заканчивающейся на гласную
    static string GetLongestSubstringWithVowels(string s)
    {
        string Vowel = "aeiouy";
        string longestSubs = string.Empty;

        for (int i = 0; i < s.Length; i++)
        {
            if (Vowel.Contains(s[i]))
            {
                for (int j = i + 1; j < s.Length; j++)
                {
                    if (Vowel.Contains(s[j]) && j > i)
                    {
                        string subs = s.Substring(i, j - i + 1);
                        if (subs.Length > longestSubs.Length)
                        {
                            longestSubs = subs;
                        }
                    }
                }
            }
        }

        return longestSubs;
    }

    static string SortResult(string result, string choice)
    {
        string sortedResult;

        if (choice == "1")
        {
            sortedResult = QuickSort(result);
            return "4. Отсортированная строка (Быстрая сортировка): " + sortedResult;
        }
        else if (choice == "2")
        {
            sortedResult = TreeSort(result);
            return "4. Отсортированная строка (Сортировка деревом): " + sortedResult;
        }
        else if (choice == "3")
        {
            string sortedResultQuick = QuickSort(result);
            string sortedResultTree = TreeSort(result);

            // Вернем результаты сортировки обоими методами в виде строки
            return $"4. 1) Отсортированная строка (Быстрая сортировка): {sortedResultQuick}\n   2) Отсортированная строка (Сортировка деревом): {sortedResultTree}";
        }
        else
        {
            return "Ошибка: Неверный выбор алгоритма сортировки.";
        }
    }

    // Быстрая сортировка
    static string QuickSort(string input)
    {
        char[] arr = input.ToCharArray();
        QuickSort(arr, 0, arr.Length - 1);
        return new string(arr);
    }

    static void QuickSort(char[] arr, int low, int high)
    {
        if (low < high)
        {
            int pop = Partition(arr, low, high);
            QuickSort(arr, low, pop - 1);
            QuickSort(arr, pop + 1, high);
        }
    }

    static int Partition(char[] arr, int low, int high)
    {
        char pivot = arr[high];
        int i = (low - 1);
        for (int j = low; j < high; j++)
        {
            if (arr[j] < pivot)
            {
                i++;
                Swap(arr, i, j);
            }
        }
        Swap(arr, i + 1, high);
        return i + 1;
    }

    static void Swap(char[] arr, int i, int j)
    {
        char temp = arr[i];
        arr[i] = arr[j];
        arr[j] = temp;
    }

    // Сортировка деревом
    static string TreeSort(string input)
    {
        Node root = null;

        foreach (char c in input)
        {
            root = Insert(root, c);
        }

        List<char> sortedChars = new List<char>();
        TraverseTreeInOrder(root, sortedChars);

        return new string(sortedChars.ToArray());
    }

    class Node
    {
        public char Data;
        public Node Left;
        public Node Right;

        public Node(char data)
        {
            Data = data;
            Left = null;
            Right = null;
        }
    }

    static Node Insert(Node root, char data)
    {
        if (root == null)
        {
            return new Node(data);
        }

        if (data < root.Data)
        {
            root.Left = Insert(root.Left, data);
        }
        else
        {
            root.Right = Insert(root.Right, data);
        }

        return root;
    }

    static void TraverseTreeInOrder(Node root, List<char> sortedChars)
    {
        if (root != null)
        {
            TraverseTreeInOrder(root.Left, sortedChars);
            sortedChars.Add(root.Data);
            TraverseTreeInOrder(root.Right, sortedChars);
        }
    }
}
    



