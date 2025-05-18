namespace Beyond.Todo.Domain.Entities;

public class Progression
{
    public int Id { get; private set; }
    public int TodoItemId { get; private set; }
    public DateTime Date { get; private set; }
    public decimal Percent { get; private set; }
    public TodoItem TodoItem { get; private set; } = null!;

    public Progression(int todoItemId, DateTime date, decimal percent)
    {
        if (percent <= 0 || percent >= 100)
            throw new ArgumentException("Percent must be > 0 and < 100");

        TodoItemId = todoItemId;
        Date = date;
        Percent = percent;
    }
}
