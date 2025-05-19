using Beyond.Todo.Application.Models;
using Beyond.Todo.Domain.Entities;

namespace Beyond.Todo.Application.Mapping;

public static class TodoItemMapping
{
    public static TodoItemDto ToDto(this TodoItem item)
    {
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
