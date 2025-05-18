using Beyond.Todo.Infrastructure;
using Mediator;

namespace Beyond.Todo.Application;

public sealed class UpdateItemHandler : IRequestHandler<UpdateItemCommand>
{
    private readonly ITodoListRepository _repo;

    public UpdateItemHandler(ITodoListRepository repo)
    {
        _repo = repo;
    }

    public async ValueTask<Unit> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _repo.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException();

        if (item.TotalProgress > 50)
            throw new InvalidOperationException("Cannot update item with more than 50% progression.");

        item.UpdateDescription(request.Description);
        await _repo.SaveChangesAsync();
        return Unit.Value;
    }
}
