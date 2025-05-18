using Beyond.Todo.Domain.Entities;

namespace Beyond.Todo.Infrastructure.Interfaces;

public interface ITodoListRepository
{
    int GetNextId();
    Task<TodoItem?> GetByIdAsync(int id);
    Task AddAsync(TodoItem item);
    Task SaveChangesAsync();
    void Remove(TodoItem item);
    Task<List<TodoItem>> GetAllItemsAsync(int skip = 0, int take = 10);
}
