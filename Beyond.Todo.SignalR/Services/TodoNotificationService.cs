using Beyond.Todo.Application.Models;
using Beyond.Todo.Application.Services;
using Beyond.Todo.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Beyond.Todo.SignalR.Services;

public class TodoNotificationService : ITodoNotificationService
{
    private readonly IHubContext<TodoHub> _hubContext;

    public TodoNotificationService(IHubContext<TodoHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyTodoItemAdded(TodoItemDto todo)
    {
        await _hubContext.Clients.All.SendAsync(TodoHub.TodoItemAdded, todo);
    }

    public async Task NotifyTodoItemUpdated(TodoItemDto todo)
    {
        await _hubContext.Clients.All.SendAsync(TodoHub.TodoItemUpdated, todo);
    }

    public async Task NotifyTodoItemDeleted(int todoId)
    {
        await _hubContext.Clients.All.SendAsync(TodoHub.TodoItemDeleted, todoId);
    }

    public async Task NotifyProgressionRegistered(int todoId, int progression)
    {
        await _hubContext.Clients.All.SendAsync(TodoHub.ProgressionRegistered, todoId, progression);
    }
}
