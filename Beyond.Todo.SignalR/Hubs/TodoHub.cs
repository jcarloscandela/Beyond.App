using Microsoft.AspNetCore.SignalR;

namespace Beyond.Todo.SignalR.Hubs;

public class TodoHub : Hub
{
    public const string TodoItemAdded = "TodoItemAdded";
    public const string TodoItemUpdated = "TodoItemUpdated";
    public const string TodoItemDeleted = "TodoItemDeleted";
    public const string ProgressionRegistered = "ProgressionRegistered";
}
