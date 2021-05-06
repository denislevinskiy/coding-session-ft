using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using CodingSessionFT.PollyClient.Repositories;
using Polly;

namespace CodingSessionFT.PollyClient
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            // await RunRetryExampleAsync();
            await RunCircuitBreakerExampleAsync();
        }

        private static async Task RunRetryExampleAsync()
        {
            var retryPolicy = Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(1, retryAttempt => TimeSpan.FromSeconds(3));

            var employeesRepository = new EmployeesRepository(new HttpClient());

            for (var i = 1; i <= 75; i++)
            {
                var sw = Stopwatch.StartNew();
                try
                {
                    var employee = await retryPolicy.ExecuteAsync(async () => await employeesRepository.GetAsync(i));
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
            var circuitBreakerPolicy = Policy.Handle<HttpRequestException>()
                .CircuitBreakerAsync(
                    1,
                    TimeSpan.FromSeconds(2),
                    (ex, t) =>
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Circuit is broken");
                        Console.ResetColor();
                    },
                    () =>
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Circuit is reset");
                        Console.ResetColor();
                    });

            var employeesRepository = new EmployeesRepository(new HttpClient());

            for (var i = 1; i <= 75; i++)
            {
                var sw = Stopwatch.StartNew();
                try
                {
                    var employee = await circuitBreakerPolicy.ExecuteAsync(async () => await employeesRepository.GetAsync(i));
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
