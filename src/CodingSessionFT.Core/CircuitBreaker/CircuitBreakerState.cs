namespace CodingSessionFT.Core.CircuitBreaker
{
    public enum CircuitBreakerState
    {
        Closed = 0,
        HalfOpen = 1,
        Open = 2,
    }
}
