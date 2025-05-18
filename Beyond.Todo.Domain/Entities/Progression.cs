namespace Beyond.Todo.Domain.Entities;

public class Progression
{
    public DateTime Date { get; private set; }
    public decimal Percent { get; private set; }

    public Progression(DateTime date, decimal percent)
    {
        Date = date;
        Percent = percent;
    }
}