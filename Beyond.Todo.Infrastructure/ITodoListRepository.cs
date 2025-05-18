using Beyond.Todo.Domain.Entities;

namespace Beyond.Todo.Infrastructure;

public interface ITodoListRepository
{
    int GetNextId();
    Task<List<string>> GetAllCategoriesAsync();
    Task<TodoItem?> GetByIdAsync(int id);
    Task AddAsync(TodoItem item);
    Task SaveChangesAsync();
    Task RemoveAsync(TodoItem item);
}

