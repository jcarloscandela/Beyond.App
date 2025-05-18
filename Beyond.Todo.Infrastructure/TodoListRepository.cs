using Beyond.Todo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Beyond.Todo.Infrastructure;

public class TodoListRepository : ITodoListRepository
{
    private readonly TodoDbContext _context;
    private int _currentId = 0;
    private readonly List<string> _categories = new() { "Work", "Personal", "Urgent" };

    public TodoListRepository(TodoDbContext context)
    {
        _context = context;
    }

    public int GetNextId() => ++_currentId;

    public Task<List<string>> GetAllCategoriesAsync() => Task.FromResult(_categories);

    public async Task<TodoItem?> GetByIdAsync(int id) =>
        await _context.TodoItems.Include(x => x.Progressions).FirstOrDefaultAsync(x => x.Id == id);

    public async Task AddAsync(TodoItem item)
    {
        await _context.TodoItems.AddAsync(item);
    }

    public async Task RemoveAsync(TodoItem item)
    {
        _context.TodoItems.Remove(item);
    }

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
