using Beyond.Todo.Domain.Entities;
using Beyond.Todo.Infrastructure;
using NSubstitute;

namespace Beyond.Todo.Application.Tests;

public class AddTodoItemHandlerTests
{
    private readonly ITodoListRepository _repo = Substitute.For<ITodoListRepository>();
    private readonly AddTodoItemHandler _handler;

    public AddTodoItemHandlerTests()
    {
        _handler = new AddTodoItemHandler(_repo);
    }

    [Fact]
    public async Task Handle_ValidCategory_AddsItemAndReturnsId()
    {
        // Arrange
        var command = new AddTodoItemCommand("Test Title", "Test Description", "Work");
        var expectedCategories = new List<string> { "Work", "Personal" };
        var expectedId = 42;

        _repo.GetAllCategoriesAsync().Returns(expectedCategories);
        _repo.GetNextId().Returns(expectedId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(expectedId, result);
        await _repo.Received(1).AddAsync(Arg.Is<TodoItem>(item =>
            item.Id == expectedId &&
            item.Title == "Test Title" &&
            item.Description == "Test Description" &&
            item.Category == "Work"
        ));
        await _repo.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_InvalidCategory_ThrowsArgumentException()
    {
        // Arrange  
        var command = new AddTodoItemCommand("Title", "Desc", "InvalidCategory");
        _repo.GetAllCategoriesAsync().Returns(["Work", "Personal"]);

        // Act & Assert  
        var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Invalid category.", ex.Message);

        await _repo.DidNotReceive().AddAsync(Arg.Any<TodoItem>());
        await _repo.DidNotReceive().SaveChangesAsync();
    }
}
