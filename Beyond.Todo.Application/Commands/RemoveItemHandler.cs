using Beyond.Todo.Domain.Exceptions;
using Beyond.Todo.Infrastructure.Interfaces;
using MediatR;

namespace Beyond.Todo.Application.Commands;

public sealed class RemoveItemHandler : IRequestHandler<RemoveItemCommand, Unit>
{
    private readonly ITodoListRepository _repo;

    public RemoveItemHandler(ITodoListRepository repo)
    {
        _repo = repo;
    }

    public async Task<Unit> Handle(RemoveItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _repo.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException();

        if (item.TotalProgress > 50)
            throw new TodoException("Cannot remove item with more than 50% progression.");

        _repo.Remove(item);
        await _repo.SaveChangesAsync();
        return Unit.Value;
    }
}
