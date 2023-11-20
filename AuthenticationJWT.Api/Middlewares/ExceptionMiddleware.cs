namespace AuthenticationJWT.Api.Middlewares;

public class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> logger;
    private readonly IHostEnvironment environment;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
    {
        this.logger = Guard.ArgumentNotNull(logger, nameof(logger));
        this.environment = Guard.ArgumentNotNull(environment, nameof(environment));
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
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
        ErrorDetails<string> errorDetails;

        if (!environment.IsDevelopment())
        {
            errorDetails = new ErrorDetails<string>
            {
                StatusCode = response.StatusCode,
                TraceId = traceId,
            };
            await context.Response.WriteAsJsonAsync(errorDetails);
        }
        errorDetails = new ErrorDetails<string>
        {
            StatusCode = response.StatusCode,
            TraceId = traceId,
            StackTrace = ex.StackTrace
        };

        apiResponse.ErrorDetails = errorDetails;

        await context.Response.WriteAsJsonAsync(apiResponse);
    }
}
