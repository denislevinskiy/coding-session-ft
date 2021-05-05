using System;
using CodingSessionFT.Client.Repositories;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using CodingSessionFT.Core.Retry;
using HttpClient = CodingSessionFT.Core.HttpClient.HttpClient;

namespace CodingSessionFT.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await RunRetryExampleAsync();
        }

        private static async Task RunRetryExampleAsync()
        {
            var retryPolicy = new RetryPolicy()
                .WithMaxRetryCount(3)
                .WithRetryInterval(TimeSpan.FromMilliseconds(500), 2)
                .HandleException<HttpRequestException>();

            var client = new HttpClient().WithRetry(retryPolicy);

            var employeesRepository = new EmployeesRepository(client);

            for (var i = 1; i <= 75; i++)
            {
                var sw = Stopwatch.StartNew();
                try
                {
                    var employee = await employeesRepository.GetAsync(i);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"Data received. Name: {employee.Name}, age: {employee.Age}; time elapsed: {sw.ElapsedMilliseconds} ms");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{ex.Message}; time elapsed: {sw.ElapsedMilliseconds} ms");
                    Console.ResetColor();
                }
                finally
                {
                    sw.Stop();
                }
            }
        }
    }
}
