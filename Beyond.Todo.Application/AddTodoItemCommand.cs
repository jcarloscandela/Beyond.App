using Mediator;

namespace Beyond.Todo.Application;

public sealed record AddTodoItemCommand(string Title, string Description, string Category) : IRequest<int>;
