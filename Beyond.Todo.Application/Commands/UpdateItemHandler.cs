using Beyond.Todo.Application.Mapping;
using Beyond.Todo.Application.Services;
using Beyond.Todo.Infrastructure.Interfaces;
using MediatR;

namespace Beyond.Todo.Application.Commands;

public sealed class UpdateItemHandler : IRequestHandler<UpdateItemCommand, Unit>
{
    private readonly ITodoListRepository _repo;
    private readonly ITodoNotificationService _notificationService;

    public UpdateItemHandler(
        ITodoListRepository repo,
        ITodoNotificationService notificationService)
    {
        _repo = repo;
        _notificationService = notificationService;
    }

    public async Task<Unit> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _repo.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException();

        if (item.TotalProgress > 50)
            throw new InvalidOperationException("[Cannot update item with more than 50% progression.]");

        item.UpdateDescription(request.Description);
        await _repo.SaveChangesAsync();

        // Notify clients about the updated todo item
        var updatedItem = await _repo.GetByIdAsync(request.Id);
        await _notificationService.NotifyTodoItemUpdated(updatedItem.ToDto());

        return Unit.Value;
    }
}
