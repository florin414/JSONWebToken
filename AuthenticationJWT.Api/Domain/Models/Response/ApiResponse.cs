namespace AuthenticationJWT.Api.Domain.Models.Response;

public class ApiResponse<T>
{
    public T Data { get; set; }
    public bool Succeeded { get; set; }
    public string Message { get; set; }

    public static ApiResponse<T> Fail(string errorMessage) =>
        new ApiResponse<T> { Succeeded = false, Message = errorMessage };

    public static ApiResponse<T> Success(T data) =>
        new ApiResponse<T> { Succeeded = true, Data = data };

    public ErrorDetails<string> ErrorDetails { get; set; }
}
