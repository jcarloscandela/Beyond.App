using Beyond.Todo.Domain.Entities;
using Beyond.Todo.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Beyond.Todo.Infrastructure.Implementations;

public class CategoryRepository : ICategoryRepository
{
    private readonly TodoDbContext _context;

    public CategoryRepository(TodoDbContext context)
    {
        _context = context;
    }

    public Task<List<Category>> GetAllCategoriesAsync() =>
        _context.Categories.OrderBy(c => c.Name).ToListAsync();

    public Task<Category?> GetCategoryByNameAsync(string name) =>
        _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
}
