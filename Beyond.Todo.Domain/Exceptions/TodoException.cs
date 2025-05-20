namespace Beyond.Todo.Domain.Exceptions;

public class TodoException : Exception
{
    public TodoException() : base("An error occurred while processing the request.")
    {
    }

    public TodoException(string message) : base(message)
    {
    }
}
