using System;
using System.Threading.Tasks;

namespace CodingSessionFT.Core.Retry
{
    public class Retry<TOutput>
    {
        private readonly RetryPolicy _policy;

        public Retry(RetryPolicy policy)
        {
            _policy = policy;
        }

        public virtual async Task<TOutput> ExecuteAsync(Func<Task<TOutput>> action)
        {
            TOutput output = default;
            var errorsCount = 0;
            var interval = _policy.RetryInterval;

            while (errorsCount <= _policy.MaxRetryCount)
            {
                try
                {
                    output = await action();
                    break;
                }
                catch (Exception ex) when (ShouldCatch(ex))
                {
                    errorsCount++;
                    if (errorsCount == _policy.MaxRetryCount)
                    {
                        throw;
                    }

                    await Task.Delay(interval);
                    interval = TimeSpan.FromMilliseconds(interval.TotalMilliseconds * _policy.IntervalMultiplier);
                }
            }

            return output;
        }

        private bool ShouldCatch(Exception ex) => _policy.DetectExceptionTypes.Contains(ex.GetType());
    }
}
