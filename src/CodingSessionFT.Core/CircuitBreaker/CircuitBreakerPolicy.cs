using System;
using System.Collections.Generic;

namespace CodingSessionFT.Core.CircuitBreaker
{
    public class CircuitBreakerPolicy
    {
        public CircuitBreakerPolicy()
        {
            MaxErrorsCount = 3;
            Timeout = TimeSpan.FromSeconds(2);
            ExceptionTypes = new List<Type>();
        }

        public int MaxErrorsCount { get; protected set; }

        public TimeSpan Timeout { get; protected set; } 

        public List<Type> ExceptionTypes { get; protected set; }

        public CircuitBreakerPolicy WithMaxErrorsCount(int count)
        {
            MaxErrorsCount = count == 0 ? 3 : count;
            return this;
        }

        public CircuitBreakerPolicy WithTimeout(TimeSpan timeout)
        {
            Timeout = timeout;
            return this;
        }

        public CircuitBreakerPolicy HandleException<TException>()
            where TException : Exception
        {
            ExceptionTypes.Add(typeof(TException));
            return this;
        }
    }
}