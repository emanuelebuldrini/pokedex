using Polly.Retry;
using Polly;

namespace Pokedex.Infrastructure.Resilience
{
    public static class PollyHttpRequestHelper
    {
        public static AsyncRetryPolicy CreatePolicy(BackoffStrategy strategy, int retryCount,
            double baseDelaySeconds, Action<Exception, TimeSpan, int, Context> onRetryAction)
        {
            return strategy switch
            {
                BackoffStrategy.Linear => Policy
                    .Handle<HttpRequestException>()
                    .WaitAndRetryAsync(
                        retryCount,
                        attempt => TimeSpan.FromSeconds(baseDelaySeconds * attempt),
                        onRetry: onRetryAction),
                BackoffStrategy.Exponential => Policy
                    .Handle<HttpRequestException>()
                    .WaitAndRetryAsync(
                        retryCount,
                        attempt => TimeSpan.FromSeconds(Math.Pow(baseDelaySeconds, attempt)),
                        onRetry: onRetryAction),
                BackoffStrategy.Constant => Policy
                    .Handle<HttpRequestException>()
                    .WaitAndRetryAsync(
                        retryCount,
                        _ => TimeSpan.FromSeconds(baseDelaySeconds),
                        onRetry: onRetryAction),
                _ => throw new ArgumentException($"Unknown retry strategy: {strategy}")
            };
        }        
    }
}
