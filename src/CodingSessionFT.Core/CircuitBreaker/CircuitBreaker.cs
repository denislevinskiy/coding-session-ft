using System;
using System.Threading.Tasks;

namespace CodingSessionFT.Core.CircuitBreaker
{
    public class CircuitBreaker<TOutput>
    {
        public CircuitBreakerPolicy Policy { get; private set; }

        private int _currentErrorsCount;

        private Exception _lastException;

        private CircuitBreakerState _state = CircuitBreakerState.Closed;

        private DateTimeOffset _updatedAt;

        public CircuitBreaker(CircuitBreakerPolicy policy)
        {
            Policy = policy;
        }

        public virtual async Task<TOutput> ExecuteAsync(Func<Task<TOutput>> action)
        {
            TOutput output;

            switch (_state)
            {
                case CircuitBreakerState.Closed:
                    {
                        try
                        {
                            output = await action();
                            break;
                        }
                        catch (Exception ex) when (ShouldCatch(ex))
                        {
                            Track(ex);
                            throw;
                        }
                    }
                case CircuitBreakerState.HalfOpen:
                case CircuitBreakerState.Open when TimeOutExpired():
                    {
                        try
                        {
                            _state = CircuitBreakerState.HalfOpen;
                            output = await action();
                            Reset();
                            break;
                        }
                        catch (Exception ex) when (ShouldCatch(ex))
                        {
                            ReOpen(ex);
                            throw;
                        }
                    }
                case CircuitBreakerState.Open when !TimeOutExpired():
                    throw new CircuitBreakerException(_lastException);
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return output;
        }

        private void Track(Exception ex)
        {
            _currentErrorsCount++;

            if (_currentErrorsCount >= Policy.MaxErrorsCount)
            {
                _lastException = ex;
                _state = CircuitBreakerState.Open;
                _updatedAt = DateTimeOffset.UtcNow;
            }
        }

        private void ReOpen(Exception ex)
        {
            _state = CircuitBreakerState.Open;
            _updatedAt = DateTime.UtcNow;
            _currentErrorsCount = 0;
            _lastException = ex;
        }

        private void Reset()
        {
            _currentErrorsCount = 0;
            _lastException = null;
            _state = CircuitBreakerState.Closed;
        }

        private bool TimeOutExpired() => _updatedAt + Policy.Timeout < DateTimeOffset.UtcNow;

        private bool ShouldCatch(Exception ex) => Policy.ExceptionTypes.Contains(ex.GetType());
    }
}
