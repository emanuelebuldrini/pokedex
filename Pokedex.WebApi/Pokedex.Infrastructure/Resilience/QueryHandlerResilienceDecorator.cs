using JewelArchitecture.Core.Application.Queries;
using JewelArchitecture.Core.Application.QueryHandlers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly.Retry;

namespace Pokedex.Infrastructure.Resilience;

public class QueryHandlerResilienceDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : IQuery
{
    private readonly AsyncRetryPolicy _retryPolicy;
    private readonly IQueryHandler<TQuery, TResult> _decoratee;
    private readonly ILogger<QueryHandlerResilienceDecorator<TQuery, TResult>> _logger;

    public QueryHandlerResilienceDecorator(IQueryHandler<TQuery, TResult> decoratee,
        IOptions<RetryPolicyOptions> retryStrategyOptions,
        ILogger<QueryHandlerResilienceDecorator<TQuery, TResult>> logger)
    {
        _decoratee = decoratee;
        _logger = logger;

        // Define the retry policy according to the app settings.        
        var strategy = retryStrategyOptions.Value.BackoffStrategy;
        var retryCount = retryStrategyOptions.Value.RetryCount;
        var delaySeconds = retryStrategyOptions.Value.DelaySeconds;

        _retryPolicy = PollyHttpRequestHelper.CreatePolicy(strategy, retryCount, delaySeconds,
                onRetryAction: (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning(exception, $"Retry {retryCount} after {timeSpan} due to '{exception.Message}'");
                });
    }

    public async Task<TResult> HandleAsync(TQuery query)
    {
        // Execute the policy around the inner service call
        return await _retryPolicy.ExecuteAsync(() => _decoratee.HandleAsync(query));
    }
}
