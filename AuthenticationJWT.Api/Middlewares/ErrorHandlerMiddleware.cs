namespace AuthenticationJWT.Api.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly ILogger<ErrorHandlerMiddleware> logger;
    private readonly RequestDelegate next;

    public ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger, RequestDelegate next)
    {
        this.logger = Guard.ArgumentNotNull(logger, nameof(logger));
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var traceId = Guid.NewGuid();
            logger.LogError(
                $"Error occure while processing the request, TraceId : ${traceId}, Message : ${ex.Message}, StackTrace: ${ex.StackTrace}"
            );

            await HandleExceptionAsync(context, ex, traceId);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex, Guid traceId)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = ex switch
        {
            BaseException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError,
        };

        var apiResponse = ApiResponse<string>.Fail(ex.Message);

        var errorDetails = new ErrorDetails<string>
        {
            StatusCode = response.StatusCode,
            TraceId = traceId,
        };

        apiResponse.ErrorDetails = errorDetails;

        await context.Response.WriteAsJsonAsync(apiResponse);
    }
}
