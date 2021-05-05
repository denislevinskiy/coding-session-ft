using System;
using System.Net.Http;
using System.Threading.Tasks;
using CodingSessionFT.Core.CircuitBreaker;

namespace CodingSessionFT.Core.HttpClient
{
    public sealed class HttpCircuitBreaker : CircuitBreaker<HttpResponseMessage>
    {
        private readonly HttpCircuitBreakerPolicy _policy;

        public HttpCircuitBreaker(HttpCircuitBreakerPolicy policy) 
            : base(policy)
        {
            _policy = policy;
        }

        public override async Task<HttpResponseMessage> ExecuteAsync(Func<Task<HttpResponseMessage>> action)
        {
            return await base.ExecuteAsync(async () =>
            {
                var response = await action();
                if (!response.IsSuccessStatusCode && _policy.StatusCodes.Contains(response.StatusCode))
                {
                    throw new HttpRequestException($"Response status code is '{response.StatusCode}'");
                }
                return response;
            });
        }
    }
}
