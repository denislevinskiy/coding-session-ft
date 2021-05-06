using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CodingSessionFT.Core.DTO;
using HttpClient = CodingSessionFT.Core.HttpClient.HttpClient;

namespace CodingSessionFT.Client.Repositories
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

        public async Task<EmployeeModel> GetWithDurableErrorEmulationAsync(int id)
        {
            _invokesCount++;
            HttpResponseMessage response;

            if ((_invokesCount >= 10 && _invokesCount <= 20) || (_invokesCount >= 25 && _invokesCount <= 40))
            {
                response = await _httpClient.SendAsync(new HttpRequestMessage
                {
                    RequestUri = new Uri($"https://localhost:5001/api/employees/{id}?error=503"),
                });
            }
            else
            {
                response = await _httpClient.SendAsync(new HttpRequestMessage
                {
                    RequestUri = new Uri($"https://localhost:5001/api/employees/{id}"),
                });
            }

            return await JsonSerializer.DeserializeAsync<EmployeeModel>(
                await response.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });
        }
    }
}
