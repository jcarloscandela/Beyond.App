using Beyond.Todo.Application.Models;

namespace Beyond.Todo.Application.Services;

public interface ITodoNotificationService
{
    Task NotifyTodoItemAdded(TodoItemDto todo);
    Task NotifyTodoItemUpdated(TodoItemDto todo);
    Task NotifyTodoItemDeleted(int todoId);
    Task NotifyProgressionRegistered(int todoId, int progression);
}
