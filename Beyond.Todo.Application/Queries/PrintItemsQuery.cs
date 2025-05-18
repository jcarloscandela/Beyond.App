using Beyond.Todo.Application.Models;
using MediatR;

namespace Beyond.Todo.Application.Queries;

public sealed record PrintItemsQuery() : IRequest<List<TodoItemDto>>;
