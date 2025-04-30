using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using StringProcessor.API.Models.Config;

namespace StringProcessor.API.Utilities
{
    public interface IRandomNumberGenerator
    {
        Task<int> GetRandomPosition(int maxLength);
    }

    public class RandomNumberGenerator : IRandomNumberGenerator
    {
        private readonly HttpClient _httpClient = new();
        private readonly Random _random = new();
        private readonly string _apiUrl;

        public RandomNumberGenerator(IOptions<RandomApiConfig> config)
        {
            _apiUrl = config.Value.RandomApi ??
                     "http://www.randomnumberapi.com/api/v1.0/random"; // fallback
        }

        public async Task<int> GetRandomPosition(int maxLength)
        {
            if (maxLength <= 0)
                throw new ArgumentException("maxLength must be positive", nameof(maxLength));

            try
            {
                string url = $"{_apiUrl}?min=0&max={maxLength - 1}&count=1";
                var response = await _httpClient.GetStringAsync(url);
                var numbers = JsonSerializer.Deserialize<List<int>>(response);

                return numbers?.FirstOrDefault() ?? GenerateLocalRandom(maxLength);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при запросе к Random API: {ex.Message}");
                return GenerateLocalRandom(maxLength);
            }
        }

        private int GenerateLocalRandom(int maxLength)
        {
            return _random.Next(0, maxLength);
        }
    }
}