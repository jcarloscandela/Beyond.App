using Beyond.Todo.Application.Models;
using MediatR;

namespace Beyond.Todo.Application.Queries
{
    public record GetTodoByIdQuery(int Id) : IRequest<TodoItemDto>;
}
