using System.Net;

namespace GoodBurger.Api.Domain.Common;

public record Error(HttpStatusCode Code, string Message)
{
    public static readonly Error None = new(HttpStatusCode.OK, string.Empty);

    public static Error NotFound(string message) => new(HttpStatusCode.NotFound, message);
    public static Error Validation(string message) => new(HttpStatusCode.BadRequest, message);
    public static Error Conflict(string message) => new(HttpStatusCode.Conflict, message);
    public static Error Forbidden(string message) => new(HttpStatusCode.Forbidden, message);
    public static Error Unauthorized(string message) => new(HttpStatusCode.Unauthorized, message);
    public static Error Failure(string message) => new(HttpStatusCode.InternalServerError, message);
}
