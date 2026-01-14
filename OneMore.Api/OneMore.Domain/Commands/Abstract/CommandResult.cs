using System.Net;

namespace OneMore.Domain.Commands.Abstract;

public interface ICommandResult
{
    HttpStatusCode GetStatusCode();
    bool Succeeded();
    string GetMessage();
    object? GetData();
    CommandResult GetResult();
}

public class CommandResult(HttpStatusCode httpStatusCode,
                    bool success,
                    string? message = null,
                    object? data = null,
                    string? exceptionMessage = null,
                    string? innerExceptionMessage = null) : ICommandResult
{
    public HttpStatusCode HttpStatusCode { get; set; } = httpStatusCode;
    public bool Success { get; set; } = success;
    public string Message { get; set; } = message ?? httpStatusCode.ToString();
    public object? Data { get; set; } = data;
    public string? ExceptionMessage { get; set; } = exceptionMessage;
    public string? InnerExceptionMessage { get; set; } = innerExceptionMessage;

    public HttpStatusCode GetStatusCode() => this.HttpStatusCode;
    public bool Succeeded() => this.Success;
    public string GetMessage() => this.Message;
    public object? GetData() => this.Data;
    public CommandResult GetResult() => this;
}

public class SuccessCommandResult(
    object? data = null,
    string? message = null) : CommandResult(HttpStatusCode.OK, true, message, data, null, null)
{
}

public class NoContentCommandResult(
    object? data = null,
    string? message = null) : CommandResult(HttpStatusCode.NoContent, false, message, data)
{
}

public class BadRequestCommandResult(
    object? data = null,
    string? message = null) : CommandResult(HttpStatusCode.BadRequest, false, message, data, null, null)
{
}

public class NotFoundCommandResult(
    object? data = null,
    string? message = null) : CommandResult(HttpStatusCode.NotFound, false, message, data)
{
}

public class UnauthorizedCommandResult(
    object? data = null,
    string? message = null) : CommandResult(HttpStatusCode.Unauthorized, false, message, data)
{
}

public class ErrorCommandResult(
    object? data = null,
    string? message = null,
    string? exceptionMessage = null,
    string? innerExceptionMessage = null) : CommandResult(HttpStatusCode.InternalServerError, false, message, data, exceptionMessage, innerExceptionMessage)
{
}

public class ExceptionCommandResult(Exception ex) : CommandResult(HttpStatusCode.InternalServerError,
        false,
        "Houve uma falha inesperada.",
        null,
        ex.Message,
        ex.InnerException?.Message)
{
}

public class DeveloperCommandResult : CommandResult
{
    public DeveloperCommandResult()
        : base(HttpStatusCode.OK, true, "*** DEV ***", null, null, null)
    { }
}