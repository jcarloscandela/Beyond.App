using Beyond.Todo.Application.Models;
using Beyond.Todo.Application.Mapping;
using Beyond.Todo.Infrastructure.Interfaces;
using MediatR;

namespace Beyond.Todo.Application.Queries;

public sealed class GetTodosHandler : IRequestHandler<GetTodosQuery, List<TodoItemDto>>
{
    private readonly ITodoListRepository _repo;

    public GetTodosHandler(ITodoListRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<TodoItemDto>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetAllItemsAsync();

        return items.Skip(request.Skip)
            .Take(request.Take)
            .Select(item => item.ToDto())
            .ToList();
    }
}
