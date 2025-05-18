using Beyond.Todo.Application.Models;
using Beyond.Todo.Infrastructure;
using MediatR;

namespace Beyond.Todo.Application;

public sealed class PrintItemsHandler : IRequestHandler<PrintItemsQuery, List<TodoItemDto>>
{
    private readonly ITodoListRepository _repo;

    public PrintItemsHandler(ITodoListRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<TodoItemDto>> Handle(PrintItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetAllItemsAsync();

        return items.Select(item => new TodoItemDto
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            Category = item.Category.Name,
            IsCompleted = item.IsCompleted,
            Progressions = item.Progressions
                .OrderBy(p => p.Date)
                .Select((p, i) =>
                {
                    var cumulative = item.Progressions
                        .OrderBy(x => x.Date)
                        .Take(i + 1)
                        .Sum(x => x.Percent);
                    return new ProgressionDto(p.Date, cumulative);
                }).ToList()
        }).ToList();
    }
}
