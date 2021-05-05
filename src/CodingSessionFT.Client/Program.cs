using System;
using CodingSessionFT.Client.Repositories;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using CodingSessionFT.Core.CircuitBreaker;
using CodingSessionFT.Core.HttpClient;
using CodingSessionFT.Core.Retry;
using HttpClient = CodingSessionFT.Core.HttpClient.HttpClient;

namespace CodingSessionFT.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await RunCircuitBreakerExampleAsync();
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

        private static async Task RunCircuitBreakerExampleAsync()
        {
            var circuitBreakerPolicy = new HttpCircuitBreakerPolicy()
                .WithMaxErrorsCount(3)
                .WithTimeout(TimeSpan.FromSeconds(2))
                .WithStatusCodes(HttpStatusCode.ServiceUnavailable);

            var client = new HttpClient().WithCircuitBreaker(circuitBreakerPolicy);

            var employeesRepository = new EmployeesRepository(client);

            employeesRepository.ResetInvokesCount();

            for (var i = 1; i <= 75; i++)
            {
                var sw = Stopwatch.StartNew();
                try
                {
                    var employee = await employeesRepository.GetWithDurableErrorEmulationAsync(i);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"Data received. Name: {employee.Name}, age: {employee.Age}; time elapsed: {sw.ElapsedMilliseconds} ms");
                    Console.ResetColor();
                }
                catch (CircuitBreakerException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{ex.Message}; time elapsed: {sw.ElapsedMilliseconds} ms");
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
