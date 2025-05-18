using Beyond.Todo.Domain.Entities;
using Beyond.Todo.Infrastructure;
using NSubstitute;

namespace Beyond.Todo.Application.Tests;

public class AddTodoItemHandlerTests
{
    private readonly ITodoListRepository _todoRepo = Substitute.For<ITodoListRepository>();
    private readonly ICategoryRepository _categoryRepo = Substitute.For<ICategoryRepository>();
    private readonly AddTodoItemHandler _handler;

    public AddTodoItemHandlerTests()
    {
        _handler = new AddTodoItemHandler(_todoRepo, _categoryRepo);
    }

    [Fact]
    public async Task Handle_ValidCategory_AddsItemAndReturnsId()
    {
        // Arrange
        var command = new AddTodoItemCommand("Test Title", "Test Description", "Work");
        var category = new Category(1, "Work");
        var expectedId = 42;

        _categoryRepo.GetCategoryByNameAsync("Work").Returns(category);
        _todoRepo.GetNextId().Returns(expectedId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(expectedId, result);
        await _todoRepo.Received(1).AddAsync(Arg.Is<TodoItem>(item =>
            item.Id == expectedId &&
            item.Title == "Test Title" &&
            item.Description == "Test Description" &&
            item.CategoryId == category.Id
        ));
        await _todoRepo.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_InvalidCategory_ThrowsArgumentException()
    {
        // Arrange  
        var command = new AddTodoItemCommand("Title", "Desc", "InvalidCategory");
        _categoryRepo.GetCategoryByNameAsync("InvalidCategory").Returns((Category?)null);

        // Act & Assert  
        var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Category 'InvalidCategory' not found.", ex.Message);

        await _todoRepo.DidNotReceive().AddAsync(Arg.Any<TodoItem>());
        await _todoRepo.DidNotReceive().SaveChangesAsync();
    }
}
