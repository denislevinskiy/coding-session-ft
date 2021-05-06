using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CodingSessionFT.Core.DTO;

namespace CodingSessionFT.PollyClient.Repositories
{
    public sealed class EmployeesRepository
    {
        private readonly HttpClient _httpClient;

        private int _invokesCount;

        public EmployeesRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void ResetInvokesCount()
        {
            _invokesCount = 0;
        }

        public async Task<EmployeeModel> GetAsync(int id)
        {
            _invokesCount++;

            var response = await _httpClient.SendAsync(new HttpRequestMessage
            {
                RequestUri = new Uri($"https://localhost:5001/api/employees/{id}"),
            });

            return await JsonSerializer.DeserializeAsync<EmployeeModel>(
              await response.Content.ReadAsStreamAsync(),
              new JsonSerializerOptions
              {
                  PropertyNameCaseInsensitive = true,
              });
        }
    }
}
