using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CodingSessionFT.Core.Retry;

namespace CodingSessionFT.Core.HttpClient
{
    public sealed class HttpClient
    {
        private HttpRetry _retry;
        private HttpCircuitBreaker _circuitBreaker;

        private readonly System.Net.Http.HttpClient _httpClient;

        public HttpClient()
        {
            _httpClient = new System.Net.Http.HttpClient();
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return _circuitBreaker is null
                ? _httpClient.SendAsync(request)
                : _circuitBreaker.ExecuteAsync(async () => await _httpClient.SendAsync(request));
        }

        public HttpClient WithRetry(RetryPolicy policy)
        {
            if (_retry is null)
            {
                _retry = new HttpRetry(policy
                    ?? new RetryPolicy()
                         .WithMaxRetryCount(3)
                         .WithRetryInterval(TimeSpan.FromMilliseconds(500), 2)
                         .HandleException<HttpRequestException>());
            }
            else
            {
                throw new InvalidOperationException("Retry already set for this instance of 'HttpClient' class.");
            }

            return this;
        }

        public HttpClient WithCircuitBreaker(HttpCircuitBreakerPolicy policy)
        {
            if (_circuitBreaker is null)
            {
                _circuitBreaker = new HttpCircuitBreaker(policy
                     ?? new HttpCircuitBreakerPolicy()
                         .WithMaxErrorsCount(3)
                         .WithTimeout(TimeSpan.FromSeconds(2))
                         .WithStatusCodes(HttpStatusCode.ServiceUnavailable));
            }
            else
            {
                throw new InvalidOperationException("Circuit breaker already set for this instance of 'HttpClient' class.");
            }

            return this;
        }
    }
}
