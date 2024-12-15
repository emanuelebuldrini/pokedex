using Polly.Retry;
using Polly;
using Pokedex.Infrastructure.ApiClients.Exceptions;

namespace Pokedex.Infrastructure.Resilience
{
    public static class PollyHttpRequestHelper
    {
        public static AsyncRetryPolicy CreatePolicy(BackoffStrategy strategy, int retryCount,
            double baseDelaySeconds, Action<Exception, TimeSpan, int, Context> onRetryAction) =>
            Policy.Handle<HttpRequestException>() // e.g., the external API is down or there is a network issue.
                .Or<HttpRetryableException>() // e.g. Internal server error or timeout.
                .WaitAndRetryAsync(
                    retryCount,
                    attempt => CalculateDelay(strategy, baseDelaySeconds, attempt),
                    onRetry: onRetryAction);

        private static TimeSpan CalculateDelay(BackoffStrategy strategy, double baseDelaySeconds, int attempt)
        {
            return strategy switch
            {
                BackoffStrategy.Linear => TimeSpan.FromSeconds(baseDelaySeconds * attempt),

                BackoffStrategy.Exponential => TimeSpan.FromSeconds(Math.Pow(baseDelaySeconds, attempt)),

                BackoffStrategy.Constant => TimeSpan.FromSeconds(baseDelaySeconds),

                _ => throw new ArgumentException($"Unknown retry strategy: {strategy}")
            };
        }
    }
}
