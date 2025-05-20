namespace Beyond.Todo.API.Models;

public class ApiErrorResponse
{
    public string Message { get; }
    public string Code { get; }

    public ApiErrorResponse(string message, string code)
    {
        Message = message;
        Code = code;
    }
}
