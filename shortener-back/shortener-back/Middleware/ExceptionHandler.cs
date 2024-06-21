namespace shortener_back.Middleware;

public class ExceptionHandler : IExceptionHandler
{
    private const string ContentType = "application/problem+json";

    private const string UnhandledExceptionMsg
        = "An unhandled exception has occurred while executing the request.";

    private static readonly JsonSerializerOptions SerializerOptions
        = new(JsonSerializerDefaults.Web)
        {
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

    private static string ToJson(in ProblemDetails problemDetails)
    {
        try
        {
            return JsonSerializer.Serialize(
                problemDetails,
                SerializerOptions
            );
        }
        catch (Exception ex)
        {
            return $$"""
                     {
                     	"status": 500,
                     	"title":"An exception has occurred while serializing error to JSON",
                     	"detail":"{{ex.Message}}"
                     }
                     """;
        }
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        int statusCode = exception switch
        {
            SecurityException or UnauthorizedAccessException
                => (int)HttpStatusCode.Unauthorized,

            NotSupportedException => (int)HttpStatusCode.Forbidden,

            ArgumentException or
                IOException or
                DbUpdateException or
                DbException
                => (int)HttpStatusCode.BadRequest,

            InvalidOperationException => (int)HttpStatusCode.Conflict,

            _ => (int)HttpStatusCode.InternalServerError
        };

        string reasonPhrase = ReasonPhrases.GetReasonPhrase(statusCode);
        if (String.IsNullOrWhiteSpace(reasonPhrase))
            reasonPhrase = UnhandledExceptionMsg;

        string details = ToJson(new ProblemDetails
        {
            Status = statusCode,
            Title = $"{statusCode} {reasonPhrase}",
            Detail = exception.Message,
        });

        httpContext.Response.ContentType = ContentType;
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsync(details, cancellationToken);

        return true;
    }
}
