using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

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

        public async Task<int> GetRandomPosition(int maxLength)
        {
            try
            {
                string url = $"http://www.randomnumberapi.com/api/v1.0/random?min=0&max={maxLength - 1}&count=1";
                var response = await _httpClient.GetStringAsync(url);
                var numbers = JsonSerializer.Deserialize<List<int>>(response);

                return numbers?.FirstOrDefault() ?? _random.Next(0, maxLength);
            }
            catch
            {
                return _random.Next(0, maxLength);
            }
        }
    }
}
