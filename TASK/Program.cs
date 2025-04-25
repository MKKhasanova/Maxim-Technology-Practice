using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Введите строку:");
        string input = Console.ReadLine();

        // Проверка на null или пустую строку
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Ошибка: Строка не может быть пустой!!!");
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
        Console.ReadKey();
    }

    // Метод для переворачивания строки
    static string ReverseString(string s)
    {
        char[] collect = s.ToCharArray();
        Array.Reverse(collect);
        return new string(collect);
    }
}

