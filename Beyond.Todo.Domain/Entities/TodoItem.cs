using Beyond.Todo.Domain.Exceptions;

namespace Beyond.Todo.Domain.Entities;

public class TodoItem
{
    private readonly List<Progression> _progressions = new();

    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public int CategoryId { get; private set; }
    public Category Category { get; private set; }
    public IReadOnlyCollection<Progression> Progressions => _progressions.AsReadOnly();
    public bool IsCompleted => _progressions.Sum(p => p.Percent) >= 100;
    public decimal TotalProgress => _progressions.Sum(p => p.Percent);

    public TodoItem(int id, string title, string description, int categoryId)
    {
        Id = id;
        Title = title;
        Description = description;
        CategoryId = categoryId;
    }

    public void UpdateDescription(string description)
    {
        if (_progressions.Sum(p => p.Percent) > 50)
            throw new TodoException("Cannot update an item with more than 50% completed.");
        Description = description;
    }

    public void AddProgression(DateTime date, decimal percent)
    {
        if (_progressions.Count > 0 && date <= _progressions.Max(p => p.Date))
            throw new TodoException("Progression date must be after the last one");

        if (_progressions.Sum(p => p.Percent) + percent > 100)
            throw new TodoException("Total progress cannot exceed 100%");

        _progressions.Add(new Progression(Id, date, percent));
    }
}
