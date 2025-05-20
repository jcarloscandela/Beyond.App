using Beyond.Todo.Application.Services;
using Beyond.Todo.SignalR.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Beyond.Todo.SignalR;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSignalRServices(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddScoped<ITodoNotificationService, TodoNotificationService>();

        return services;
    }
}
