using System.Net;

namespace Pokedex.Infrastructure.ApiClients.Exceptions;

public class HttpRetryableException(HttpStatusCode statusCode) : Exception
{
    public HttpStatusCode StatusCode { get; } = statusCode;
}