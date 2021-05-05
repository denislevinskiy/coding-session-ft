using System;

namespace CodingSessionFT.Core.CircuitBreaker
{
    public sealed class CircuitBreakerException : Exception
    {
        public CircuitBreakerException(Exception ex)
          : base(ex.Message, ex)
        {
        }
    }
}
