using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Beyond.Todo.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<TodoDbContext>(options =>
                options.UseInMemoryDatabase("TodoDb"));

        services.AddScoped<ITodoListRepository, TodoListRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        var sp = services.BuildServiceProvider();
        using var scope = sp.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
        context.SeedDataAsync().GetAwaiter().GetResult();

        return services;
    }
}
