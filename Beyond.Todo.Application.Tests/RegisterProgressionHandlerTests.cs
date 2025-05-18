using Beyond.Todo.Domain.Entities;
using Beyond.Todo.Infrastructure;
using Mediator;
using NSubstitute;

namespace Beyond.Todo.Application.Tests;

public class RegisterProgressionHandlerTests
{
    private readonly ITodoListRepository _repo = Substitute.For<ITodoListRepository>();
    private readonly RegisterProgressionHandler _handler;

    public RegisterProgressionHandlerTests()
    {
        _handler = new RegisterProgressionHandler(_repo);
    }

    [Fact]
    public async Task Handle_ValidItem_AddsProgressionAndSavesChanges()
    {
        // Arrange
        var command = new RegisterProgressionCommand(1, new DateTime(2025, 5, 18), 60);

        var item = new TodoItem(1, "title", "desc", "category");
        var repo = Substitute.For<ITodoListRepository>();
        repo.GetByIdAsync(command.Id).Returns(item);

        var handler = new RegisterProgressionHandler(repo);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(Unit.Value, result);
        await repo.Received(1).SaveChangesAsync();
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
    }
}
