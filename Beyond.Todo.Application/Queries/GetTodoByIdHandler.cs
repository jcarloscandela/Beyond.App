using Beyond.Todo.Application.Models;
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

        return new TodoItemDto
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
                }).ToList(),
            TotalProgression = (int)(item.Progressions.Sum(x => x.Percent))
        };
    }
}
