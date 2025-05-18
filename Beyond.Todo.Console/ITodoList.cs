namespace Beyond.Todo.Console;

public interface ITodoList
{
    void AddItem(string title, string description, string category);
    void UpdateItem(int id, string description);
    void RemoveItem(int id);
    void RegisterProgression(int id, DateTime dateTime, decimal percent);
    void PrintItems();
    Task<List<string>> GetCategories();
}
