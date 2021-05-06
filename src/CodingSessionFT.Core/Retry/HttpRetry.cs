using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodingSessionFT.Core.Retry
{
    public sealed class HttpRetry : Retry<HttpResponseMessage>
    {
        public HttpRetry(RetryPolicy policy)
            : base(policy)
        {
        }

        public async Task<HttpResponseMessage> ExecuteAsync(
            Func<HttpRequestMessage, Task<HttpResponseMessage>> action,
            HttpRequestMessage initialRequest)
        {
            return await ExecuteAsync(async () => await action(GenerateRequest(initialRequest)));
        }

        private static HttpRequestMessage GenerateRequest(HttpRequestMessage message) =>
            new HttpRequestMessage(message.Method, message.RequestUri)
            {
                Content = message.Content,
                Version = message.Version,
            };
    }
}
