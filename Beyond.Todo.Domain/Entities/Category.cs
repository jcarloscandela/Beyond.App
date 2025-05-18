namespace Beyond.Todo.Domain.Entities;

public class Category
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public ICollection<TodoItem> Items { get; private set; } = new List<TodoItem>();

    public Category(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
