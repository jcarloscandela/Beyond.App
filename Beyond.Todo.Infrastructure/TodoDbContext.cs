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

        var category1 = new Category(1, "Programming");
        var category2 = new Category(2, "Work");
        var category3 = new Category(3, "Misc");

        await Categories.AddRangeAsync(category1, category2, category3);
        await SaveChangesAsync();

        var items = new List<TodoItem>
        {
            new TodoItem(1, "Learn C#", "Master C# programming language", 1),
            new TodoItem(2, "Learn Entity Framework", "Understand EF Core and its features", 1),
            new TodoItem(3, "Build a Web API", "Create a RESTful API using ASP.NET Core", 1),
            new TodoItem(4, "Write Documentation", "Document the project architecture and APIs", 2),
            new TodoItem(5, "Code Review", "Review pending pull requests", 2),
            new TodoItem(6, "Team Meeting", "Weekly sync with the development team", 2),
            new TodoItem(7, "Deploy to Production", "Deploy latest changes to production environment", 2),
            new TodoItem(8, "Security Audit", "Perform security assessment of the application", 2),
            new TodoItem(9, "Learn Docker", "Master containerization with Docker", 1),
            new TodoItem(10, "Database Optimization", "Optimize database queries and indexes", 1),
            new TodoItem(11, "Read Tech Blog", "Stay updated with latest tech trends", 3),
            new TodoItem(12, "Exercise", "Daily workout routine", 3),
            new TodoItem(13, "Plan Next Sprint", "Plan and prioritize tasks for next sprint", 2)
        };

        // Add some progress to different items
        items[0].AddProgression(DateTime.Today.AddDays(-5), 30);
        items[0].AddProgression(DateTime.Today.AddDays(-2), 60);
        items[1].AddProgression(DateTime.Today.AddDays(-3), 40);
        items[3].AddProgression(DateTime.Today.AddDays(-1), 25);
        items[4].AddProgression(DateTime.Today.AddDays(-2), 75);
        items[11].AddProgression(DateTime.Today.AddDays(-1), 50);

        await TodoItems.AddRangeAsync(items);
        await SaveChangesAsync();
    }
}
