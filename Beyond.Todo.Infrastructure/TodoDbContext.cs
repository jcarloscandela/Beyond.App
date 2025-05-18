using Beyond.Todo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Beyond.Todo.Infrastructure;

public class TodoDbContext : DbContext
{
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>().OwnsMany(x => x.Progressions);
    }

    public async Task SeedDataAsync()
    {
        if (await TodoItems.AnyAsync())
            return;

        var item1 = new TodoItem(1, "Learn C#", "Master C# programming language", "Programming");
        item1.AddProgression(DateTime.Today.AddDays(-5), 30);
        item1.AddProgression(DateTime.Today.AddDays(-2), 60);

        var item2 = new TodoItem(2, "Learn Entity Framework", "Understand EF Core and its features", "Programming");
        item2.AddProgression(DateTime.Today.AddDays(-3), 40);

        var item3 = new TodoItem(3, "Build a Web API", "Create a RESTful API using ASP.NET Core", "Programming");

        await TodoItems.AddRangeAsync(item1, item2, item3);
        await SaveChangesAsync();
    }
}
