using Beyond.Todo.Application.Queries;
using Beyond.Todo.Domain.Entities;
using Beyond.Todo.Infrastructure.Interfaces;
using NSubstitute;

namespace Beyond.Todo.Application.Tests;

public class GetTodoByIdHandlerTests
{
    private readonly ITodoListRepository _repo = Substitute.For<ITodoListRepository>();
    private readonly GetTodoByIdHandler _handler;

    public GetTodoByIdHandlerTests()
    {
        _handler = new GetTodoByIdHandler(_repo);
    }

    [Fact]
    public async Task Handle_ExistingItem_ReturnsMappedDto()
    {
        // Arrange
        var category = new Category(1, "Work");
        var item = new TodoItem(1, "Test Task", "Test Description", category.Id);
        var now = DateTime.UtcNow;

        // Add two progressions: 30% and then 40% more
        item.AddProgression(now.AddDays(-2), 30);
        item.AddProgression(now.AddDays(-1), 40);

        // Set up the category reference (required for mapping)
        typeof(TodoItem).GetProperty(nameof(TodoItem.Category))!
            .SetValue(item, category);

        _repo.GetByIdAsync(1).Returns(item);

        // Act
        var result = await _handler.Handle(new GetTodoByIdQuery(1), CancellationToken.None);

        // Assert
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Task", result.Title);
        Assert.Equal("Test Description", result.Description);
        Assert.Equal("Work", result.Category);
        Assert.Equal(70, result.TotalProgression); // 30% + 40%
        Assert.False(result.IsCompleted);

        // Verify progressions are mapped with cumulative values
        Assert.Collection(result.Progressions.OrderBy(p => p.Date),
            p =>
            {
                Assert.Equal(now.AddDays(-2).Date, p.Date.Date);
                Assert.Equal(30, p.CumulativePercent); // First progression shows its own value
            },
            p =>
            {
                Assert.Equal(now.AddDays(-1).Date, p.Date.Date);
                Assert.Equal(70, p.CumulativePercent); // Second progression shows cumulative value
            }
        );
    }

    [Fact]
    public async Task Handle_ItemNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        _repo.GetByIdAsync(99).Returns((TodoItem?)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _handler.Handle(new GetTodoByIdQuery(99), CancellationToken.None));

        Assert.Equal("Todo item with ID 99 not found", ex.Message);
    }
}
