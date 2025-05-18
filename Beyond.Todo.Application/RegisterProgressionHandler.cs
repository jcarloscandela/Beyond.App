using Beyond.Todo.Infrastructure;
using Mediator;

namespace Beyond.Todo.Application;

public sealed class RegisterProgressionHandler : IRequestHandler<RegisterProgressionCommand>
{
    private readonly ITodoListRepository _repo;

    public RegisterProgressionHandler(ITodoListRepository repo)
    {
        _repo = repo;
    }

    public async ValueTask<Unit> Handle(RegisterProgressionCommand request, CancellationToken cancellationToken)
    {
        var item = await _repo.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException();
        item.AddProgression(request.Date, request.Percent);
        await _repo.SaveChangesAsync();
        return Unit.Value;
    }
}
