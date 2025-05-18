using Beyond.Todo.Infrastructure.Interfaces;
using MediatR;

namespace Beyond.Todo.Application.Commands;

public sealed class RegisterProgressionHandler : IRequestHandler<RegisterProgressionCommand, Unit>
{
    private readonly ITodoListRepository _repo;

    public RegisterProgressionHandler(ITodoListRepository repo)
    {
        _repo = repo;
    }

    public async Task<Unit> Handle(RegisterProgressionCommand request, CancellationToken cancellationToken)
    {
        var item = await _repo.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException();
        item.AddProgression(request.Date, request.Percent);
        await _repo.SaveChangesAsync();
        return Unit.Value;
    }
}
