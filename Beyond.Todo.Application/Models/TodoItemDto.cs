namespace Beyond.Todo.Application.Models;

public sealed class TodoItemDto
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public string Category { get; init; }
    public bool IsCompleted { get; init; }
    public List<ProgressionDto> Progressions { get; init; } = new();
    public int TotalProgression { get; init; }
}

public sealed record ProgressionDto(DateTime Date, decimal CumulativePercent);
