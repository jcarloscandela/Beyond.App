using Beyond.Todo.Application.Models;
using MediatR;

namespace Beyond.Todo.Application.Queries;

public sealed record GetTodosQuery(int Skip = 0, int Take = 10) : IRequest<List<TodoItemDto>>;
