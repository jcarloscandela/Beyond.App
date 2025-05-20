using Beyond.Todo.Application.Models;
using Beyond.Todo.Application.Queries;
using Beyond.Todo.Domain.Entities;
using Beyond.Todo.Infrastructure.Interfaces;
using NSubstitute;

namespace Beyond.Todo.Application.Tests;

public class GetTodosHandlerTests
{
    private readonly ITodoListRepository _repo = Substitute.For<ITodoListRepository>();
    private readonly GetTodosHandler _handler;

    public GetTodosHandlerTests()
    {
        _handler = new GetTodosHandler(_repo);
    }

    [Fact]
    public async Task Handle_WithItems_ReturnsPaginatedMappedDtos()
    {
        // Arrange
        var category = new Category(1, "Work");
        var items = new List<TodoItem>
        {
            CreateTodoItem(1, "Task 1", category, 30),
            CreateTodoItem(2, "Task 2", category, 60),
            CreateTodoItem(3, "Task 3", category, 100)
        };

        var query = new GetTodosQuery(0, 2);
        var totalCount = items.Count;

        // Get first 2 items for this test
        var pagedItems = items.ToList().GetRange(0, 2);

        _repo.GetAllItemsAsync(query.Skip, query.Take).Returns(pagedItems);
        _repo.CountAsync().Returns(totalCount);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(totalCount, result.TotalCount);
        Assert.Equal(query.Skip, result.Skip);
        Assert.Equal(query.Take, result.Take);
        Assert.Equal(2, result.Items.Count());

        Assert.Collection(result.Items,
            dto =>
            {
                Assert.Equal(1, dto.Id);
                Assert.Equal("Task 1", dto.Title);
                Assert.Equal("Work", dto.Category);
                Assert.Equal(30, dto.TotalProgression);
                Assert.False(dto.IsCompleted);
            },
            dto =>
            {
                Assert.Equal(2, dto.Id);
                Assert.Equal("Task 2", dto.Title);
                Assert.Equal("Work", dto.Category);
                Assert.Equal(60, dto.TotalProgression);
                Assert.False(dto.IsCompleted);
            }
        );
    }

    [Fact]
    public async Task Handle_NoItems_ReturnsEmptyPaginatedResult()
    {
        // Arrange
        var query = new GetTodosQuery(0, 10);
        _repo.GetAllItemsAsync(query.Skip, query.Take).Returns(new List<TodoItem>());
        _repo.CountAsync().Returns(0);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
        Assert.Equal(query.Skip, result.Skip);
        Assert.Equal(query.Take, result.Take);
    }

    [Fact]
    public async Task Handle_WithPagination_ReturnsCorrectPage()
    {
        // Arrange
        var category = new Category(1, "Work");
        var allItems = new List<TodoItem>
        {
            CreateTodoItem(1, "Task 1", category, 30),
            CreateTodoItem(2, "Task 2", category, 60),
            CreateTodoItem(3, "Task 3", category, 100)
        };

        var query = new GetTodosQuery(1, 1);
        var totalCount = allItems.Count;

        // Get the second item only (skip 1, take 1)
        var pagedItems = allItems.ToList().GetRange(query.Skip, query.Take);

        _repo.GetAllItemsAsync(query.Skip, query.Take).Returns(pagedItems);
        _repo.CountAsync().Returns(totalCount);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(totalCount, result.TotalCount);
        Assert.Equal(query.Skip, result.Skip);
        Assert.Equal(query.Take, result.Take);
        Assert.Single(result.Items);

        var item = Assert.Single(result.Items);
        Assert.Equal(2, item.Id);
        Assert.Equal("Task 2", item.Title);
        Assert.Equal(60, item.TotalProgression);
    }

    private static TodoItem CreateTodoItem(int id, string title, Category category, int progress)
    {
        var item = new TodoItem(id, title, "Description", category.Id);

        // Set up the category reference
        typeof(TodoItem).GetProperty(nameof(TodoItem.Category))!
            .SetValue(item, category);

        if (progress > 0)
        {
            item.AddProgression(DateTime.UtcNow, progress);
        }

        return item;
    }
}
