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

        return services;
    }
}
