using Beyond.Todo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Beyond.Todo.Infrastructure;

public class TodoListRepository : ITodoListRepository
{
    private readonly TodoDbContext _context;
    private readonly List<string> _categories = new() { "Work", "Personal", "Urgent" };

    public TodoListRepository(TodoDbContext context)
    {
        _context = context;
    }

    public int GetNextId() => _context.TodoItems.Any() ? _context.TodoItems.Max(x => x.Id) + 1 : 1;

    public Task<List<string>> GetAllCategoriesAsync() => Task.FromResult(_categories);

    public async Task<List<TodoItem>> GetAllItemsAsync(int skip = 0, int take = 10)
    {
        return await _context.TodoItems
            .Include(x => x.Progressions)
            .OrderBy(x => x.Id)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<TodoItem?> GetByIdAsync(int id) =>
        await _context.TodoItems.Include(x => x.Progressions).FirstOrDefaultAsync(x => x.Id == id);

    public async Task AddAsync(TodoItem item)
    {
        await _context.TodoItems.AddAsync(item);
    }

    public void Remove(TodoItem item)
    {
        _context.TodoItems.Remove(item);
    }

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
