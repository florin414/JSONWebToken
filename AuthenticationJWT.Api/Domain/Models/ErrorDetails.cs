namespace AuthenticationJWT.Api.Domain.Models;

public class ErrorDetails<T>
{
    public int StatusCode { get; set; }
    public Guid TraceId { get; set; }
    public string? StackTrace { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
