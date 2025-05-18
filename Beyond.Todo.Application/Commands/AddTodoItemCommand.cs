using MediatR;

namespace Beyond.Todo.Application.Commands;

public sealed record AddTodoItemCommand(string Title, string Description, string Category) : IRequest<int>;
