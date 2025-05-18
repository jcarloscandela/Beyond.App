using Beyond.Todo.Application.Models;
using MediatR;

namespace Beyond.Todo.Application;

public sealed record PrintItemsQuery() : IRequest<List<TodoItemDto>>;
