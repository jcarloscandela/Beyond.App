using Beyond.Todo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Beyond.Todo.Infrastructure;

public class TodoDbContext : DbContext
{
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public DbSet<Progression> Progressions => Set<Progression>();
    public DbSet<Category> Categories => Set<Category>();

    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(builder =>
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired();
        });

        modelBuilder.Entity<TodoItem>(builder =>
        {
            builder.HasOne(t => t.Category)
                   .WithMany(c => c.Items)
                   .HasForeignKey(t => t.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Progression>(builder =>
        {
            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.TodoItem)
                   .WithMany(t => t.Progressions)
                   .HasForeignKey(p => p.TodoItemId)
                   .OnDelete(DeleteBehavior.Cascade);
        });
    }

    public async Task SeedDataAsync()
    {
        if (await Categories.AnyAsync())
            return;

        var programmingCategory = new Category(1, "Programming");
        await Categories.AddAsync(programmingCategory);
        await SaveChangesAsync();

        var item1 = new TodoItem(1, "Learn C#", "Master C# programming language", 1);
        item1.AddProgression(DateTime.Today.AddDays(-5), 30);
        item1.AddProgression(DateTime.Today.AddDays(-2), 60);

        var item2 = new TodoItem(2, "Learn Entity Framework", "Understand EF Core and its features", 1);
        item2.AddProgression(DateTime.Today.AddDays(-3), 40);

        var item3 = new TodoItem(3, "Build a Web API", "Create a RESTful API using ASP.NET Core", 1);

        await TodoItems.AddRangeAsync(item1, item2, item3);
        await SaveChangesAsync();
    }
}
