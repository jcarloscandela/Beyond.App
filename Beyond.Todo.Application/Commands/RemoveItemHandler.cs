using Beyond.Todo.Application.Services;
using Beyond.Todo.Infrastructure.Interfaces;
using MediatR;

namespace Beyond.Todo.Application.Commands;

public sealed class RemoveItemHandler : IRequestHandler<RemoveItemCommand, Unit>
{
    private readonly ITodoListRepository _repo;
    private readonly ITodoNotificationService _notificationService;

    public RemoveItemHandler(
        ITodoListRepository repo,
        ITodoNotificationService notificationService)
    {
        _repo = repo;
        _notificationService = notificationService;
    }

    public async Task<Unit> Handle(RemoveItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _repo.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException();

        if (item.TotalProgress > 50)
            throw new InvalidOperationException("[Cannot remove item with more than 50% progression.]");

        var itemId = item.Id;
        _repo.Remove(item);
        await _repo.SaveChangesAsync();

        // Notify clients about the deleted todo item
        await _notificationService.NotifyTodoItemDeleted(itemId);

        return Unit.Value;
    }
}
