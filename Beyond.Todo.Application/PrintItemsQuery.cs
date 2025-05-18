using Beyond.Todo.Application.Models;
using Mediator;

namespace Beyond.Todo.Application;

public sealed record PrintItemsQuery() : IRequest<List<TodoItemDto>>;
