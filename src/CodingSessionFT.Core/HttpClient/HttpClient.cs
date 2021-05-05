using System;
using System.Net.Http;
using System.Threading.Tasks;
using CodingSessionFT.Core.Retry;

namespace CodingSessionFT.Core.HttpClient
{
    public sealed class HttpClient
    {
        private HttpRetry _retry;
        
        private readonly System.Net.Http.HttpClient _httpClient;

        public HttpClient()
        {
            _httpClient = new System.Net.Http.HttpClient();
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return _retry is null 
                ? _httpClient.SendAsync(request) 
                : _retry.ExecuteAsync(async rq => await _httpClient.SendAsync(rq), request);
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
    }
}
