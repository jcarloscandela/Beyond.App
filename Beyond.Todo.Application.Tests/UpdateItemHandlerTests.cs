using Beyond.Todo.Application.Commands;
using Beyond.Todo.Domain.Entities;
using Beyond.Todo.Domain.Exceptions;
using Beyond.Todo.Infrastructure.Interfaces;
using MediatR;
using NSubstitute;

namespace Beyond.Todo.Application.Tests;

public class UpdateItemHandlerTests
{
    private readonly ITodoListRepository _repo = Substitute.For<ITodoListRepository>();
    private readonly UpdateItemHandler _handler;

    public UpdateItemHandlerTests()
    {
        _handler = new UpdateItemHandler(_repo);
    }

    [Fact]
    public async Task Handle_ItemWithLowProgress_UpdatesDescriptionAndSavesChanges()
    {
        // Arrange
        var command = new UpdateItemCommand(1, "Updated description");
        var item = new TodoItem(1, "Test", "Original description", 1);
        item.AddProgression(DateTime.UtcNow, 30); // Adds 30% progress

        _repo.GetByIdAsync(command.Id).Returns(item);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(Unit.Value, result);
        Assert.Equal("Updated description", item.Description);
        await _repo.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_ItemWithHighProgress_ThrowsTodoException()
    {
        // Arrange
        var command = new UpdateItemCommand(1, "Updated description");
        var item = new TodoItem(1, "Test", "Original description", 1);
        item.AddProgression(DateTime.UtcNow, 75); // Adds 75% progress

        _repo.GetByIdAsync(command.Id).Returns(item);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<TodoException>(
            async () => await _handler.Handle(command, CancellationToken.None));

        Assert.Equal("Cannot update item with more than 50% progression.", ex.Message);
        Assert.Equal("Original description", item.Description); // Description should not change
        await _repo.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_ItemNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = new UpdateItemCommand(99, "Updated description");
        _repo.GetByIdAsync(command.Id).Returns((TodoItem?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _handler.Handle(command, CancellationToken.None));

        await _repo.DidNotReceive().SaveChangesAsync();
    }
}
