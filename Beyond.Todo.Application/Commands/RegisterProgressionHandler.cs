using Beyond.Todo.Application.Services;
using Beyond.Todo.Infrastructure.Interfaces;
using MediatR;

namespace Beyond.Todo.Application.Commands;

public sealed class RegisterProgressionHandler : IRequestHandler<RegisterProgressionCommand, Unit>
{
    private readonly ITodoListRepository _repo;
    private readonly ITodoNotificationService _notificationService;

    public RegisterProgressionHandler(
        ITodoListRepository repo,
        ITodoNotificationService notificationService)
    {
        _repo = repo;
        _notificationService = notificationService;
    }

    public async Task<Unit> Handle(RegisterProgressionCommand request, CancellationToken cancellationToken)
    {
        var item = await _repo.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException();
        item.AddProgression(request.Date, request.Percent);
        await _repo.SaveChangesAsync();

        // Notify clients about the new progression
        await _notificationService.NotifyProgressionRegistered(request.Id, (int)request.Percent);

        return Unit.Value;
    }
}
