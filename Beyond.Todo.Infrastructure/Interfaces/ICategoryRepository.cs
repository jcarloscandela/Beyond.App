using Beyond.Todo.Domain.Entities;

namespace Beyond.Todo.Infrastructure.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllCategoriesAsync();
    Task<Category?> GetCategoryByNameAsync(string name);
}
