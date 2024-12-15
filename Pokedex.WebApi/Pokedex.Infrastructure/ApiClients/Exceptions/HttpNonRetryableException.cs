namespace Pokedex.Infrastructure.ApiClients.Exceptions;

public class HttpNonRetryableException(HttpRequestException exception) 
    : HttpRequestException($"A non-retryable exception occurred. {exception.Message}", exception, 
        exception.StatusCode);
