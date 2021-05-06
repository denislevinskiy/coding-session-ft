using System;
using System.Collections.Generic;

namespace CodingSessionFT.Core.Retry
{
    public class RetryPolicy
    {
        public RetryPolicy()
        {
            MaxRetryCount = 3;
            RetryInterval = TimeSpan.FromMilliseconds(500);
            IntervalMultiplier = 2;
            DetectExceptionTypes = new List<Type>();
        }

        public List<Type> DetectExceptionTypes { get; }

        public int MaxRetryCount { get; private set; }

        public TimeSpan RetryInterval { get; private set; }

        public int IntervalMultiplier { get; private set; }

        public RetryPolicy WithMaxRetryCount(int count)
        {
            MaxRetryCount = count == 0 ? 3 : count;
            return this;
        }

        public RetryPolicy WithRetryInterval(TimeSpan interval, int multiplier)
        {
            RetryInterval = interval;
            IntervalMultiplier = multiplier == 0 ? 2 : multiplier;
            return this;
        }

        public RetryPolicy HandleException<TException>()
            where TException : Exception
        {
            DetectExceptionTypes.Add(typeof(TException));
            return this;
        }
    }
}
