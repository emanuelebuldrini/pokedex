﻿namespace Pokedex.Infrastructure.ApiClients.Exceptions;

public class HttpRetryableException(HttpRequestException exception)
    : HttpRequestException($"A retryable exception occurred. {exception.Message}", exception,
        exception.StatusCode);