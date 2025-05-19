using Beyond.Todo.Application.Models;
using Beyond.Todo.Application.Mapping;
using Beyond.Todo.Infrastructure.Interfaces;
using MediatR;

namespace Beyond.Todo.Application.Queries;

public sealed class GetTodosHandler : IRequestHandler<GetTodosQuery, PaginatedTodoItemsDto>
{
    private readonly ITodoListRepository _repo;

    public GetTodosHandler(ITodoListRepository repo)
    {
        _repo = repo;
    }

    public async Task<PaginatedTodoItemsDto> Handle(GetTodosQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetAllItemsAsync(request.Skip, request.Take);
        var totalCount =  await _repo.CountAsync();

        var paginatedItems = items
            .Select(item => item.ToDto())
            .ToList();

        return new PaginatedTodoItemsDto
        {
            Items = paginatedItems,
            TotalCount = totalCount,
            Skip = request.Skip,
            Take = request.Take
        };
    }
}
