using Beyond.Todo.Application.Commands;
using Beyond.Todo.Domain.Entities;
using Beyond.Todo.Domain.Exceptions;
using Beyond.Todo.Infrastructure.Interfaces;
using MediatR;
using NSubstitute;

namespace Beyond.Todo.Application.Tests;

public class RemoveItemHandlerTests
{
    private readonly ITodoListRepository _repo = Substitute.For<ITodoListRepository>();
    private readonly RemoveItemHandler _handler;

    public RemoveItemHandlerTests()
    {
        _handler = new RemoveItemHandler(_repo);
    }

    [Fact]
    public async Task Handle_ItemWithLowProgress_RemovesItemAndSavesChanges()
    {
        // Arrange
        var command = new RemoveItemCommand(1);
        var item = new TodoItem(1, "Test", "Description", 1);
        item.AddProgression(DateTime.UtcNow, 30); // Adds 30% progress

        _repo.GetByIdAsync(command.Id).Returns(item);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(Unit.Value, result);
        _repo.Received(1).Remove(item);
        await _repo.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_ItemWithHighProgress_ThrowsTodoException()
    {
        // Arrange
        var command = new RemoveItemCommand(1);
        var item = new TodoItem(1, "Test", "Description", 1);
        item.AddProgression(DateTime.UtcNow, 75); // Adds 75% progress

        _repo.GetByIdAsync(command.Id).Returns(item);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<TodoException>(
            async () => await _handler.Handle(command, CancellationToken.None));

        Assert.Equal("Cannot remove item with more than 50% progression.", ex.Message);
        _repo.DidNotReceive().Remove(Arg.Any<TodoItem>());
        await _repo.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_ItemNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = new RemoveItemCommand(99);
        _repo.GetByIdAsync(command.Id).Returns((TodoItem?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _handler.Handle(command, CancellationToken.None));

        _repo.DidNotReceive().Remove(Arg.Any<TodoItem>());
        await _repo.DidNotReceive().SaveChangesAsync();
    }
}
