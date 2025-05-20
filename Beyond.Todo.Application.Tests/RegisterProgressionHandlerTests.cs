using Beyond.Todo.Application.Commands;
using Beyond.Todo.Application.Services;
using Beyond.Todo.Domain.Entities;
using Beyond.Todo.Infrastructure.Interfaces;
using MediatR;
using NSubstitute;

namespace Beyond.Todo.Application.Tests;

public class RegisterProgressionHandlerTests
{
    private readonly ITodoListRepository _repo;
    private readonly ITodoNotificationService _notificationService;
    private readonly RegisterProgressionHandler _handler;

    public RegisterProgressionHandlerTests()
    {
        _repo = Substitute.For<ITodoListRepository>();
        _notificationService = Substitute.For<ITodoNotificationService>();
        _handler = new RegisterProgressionHandler(_repo, _notificationService);
    }

    [Fact]
    public async Task Handle_ValidItem_AddsProgressionAndSavesChanges()
    {
        // Arrange
        var command = new RegisterProgressionCommand(1, new DateTime(2025, 5, 18), 60);

        var item = new TodoItem(1, "title", "desc", 1);
        _repo.GetByIdAsync(command.Id).Returns(item);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(Unit.Value, result);
        await _repo.Received(1).SaveChangesAsync();
        await _notificationService.Received(1).NotifyProgressionRegistered(1, 60);
    }


    [Fact]
    public async Task Handle_ItemNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = new RegisterProgressionCommand(99, DateTime.UtcNow, 75);
        _repo.GetByIdAsync(command.Id).Returns((TodoItem?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));

        await _repo.DidNotReceive().SaveChangesAsync();
        await _notificationService.DidNotReceive().NotifyProgressionRegistered(Arg.Any<int>(), Arg.Any<int>());
    }
}
