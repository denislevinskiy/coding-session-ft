using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using CodingSessionFT.Core.CircuitBreaker;

namespace CodingSessionFT.Core.HttpClient
{
    public sealed class HttpCircuitBreakerPolicy : CircuitBreakerPolicy
    {
        public HttpCircuitBreakerPolicy()
        {
            StatusCodes = new List<HttpStatusCode> { HttpStatusCode.ServiceUnavailable };
            ExceptionTypes.Add(typeof(HttpRequestException));
        }

        public List<HttpStatusCode> StatusCodes { get; }

        public HttpCircuitBreakerPolicy WithStatusCodes(params HttpStatusCode[] statusCodes)
        {
            this.StatusCodes.AddRange(statusCodes);
            return this;
        }

        public new HttpCircuitBreakerPolicy WithMaxErrorsCount(int count)
        {
            MaxErrorsCount = count == 0 ? 3 : count;
            return this;
        }

        public new HttpCircuitBreakerPolicy WithTimeout(TimeSpan timeout)
        {
            Timeout = timeout;
            return this;
        }

        public new HttpCircuitBreakerPolicy HandleException<TException>()
            where TException : Exception
        {
            ExceptionTypes.Add(typeof(TException));
            return this;
        }
    }
}
