using System.Net;

namespace LMS.Blazor.Client.Exceptions;

public class ApiException : HttpRequestException
{
    public HttpStatusCode StatusCode { get; }
    public string? ProblemDetails { get; }

    public ApiException(string message, HttpStatusCode statusCode, string? problemDetails = null)
        : base(message)
    {
        StatusCode = statusCode;
        ProblemDetails = problemDetails;
    }
}
