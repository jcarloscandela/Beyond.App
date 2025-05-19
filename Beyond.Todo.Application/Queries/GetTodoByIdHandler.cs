using Beyond.Todo.Application.Models;
using Beyond.Todo.Application.Mapping;
using Beyond.Todo.Infrastructure.Interfaces;
using MediatR;

namespace Beyond.Todo.Application.Queries;

public sealed class GetTodoByIdHandler : IRequestHandler<GetTodoByIdQuery, TodoItemDto>
{
    private readonly ITodoListRepository _repo;

    public GetTodoByIdHandler(ITodoListRepository repo)
    {
        _repo = repo;
    }

    public async Task<TodoItemDto> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _repo.GetByIdAsync(request.Id);

        if (item == null)
            throw new KeyNotFoundException($"Todo item with ID {request.Id} not found");

        return item.ToDto();
    }
}
