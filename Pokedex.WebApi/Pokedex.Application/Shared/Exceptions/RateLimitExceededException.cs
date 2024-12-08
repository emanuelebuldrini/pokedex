namespace Pokedex.Application.Shared.Exceptions;

public class RateLimitExceededException(string apiName, string? rateLimitDetails = null)
    : Exception($"Unable to process the request because the {apiName} API rate limit was reached. {rateLimitDetails}");
