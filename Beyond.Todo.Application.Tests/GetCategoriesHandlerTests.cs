using Beyond.Todo.Application.Queries;
using Beyond.Todo.Domain.Entities;
using Beyond.Todo.Infrastructure.Interfaces;
using NSubstitute;

namespace Beyond.Todo.Application.Tests;

public class GetCategoriesHandlerTests
{
    private readonly ICategoryRepository _repository = Substitute.For<ICategoryRepository>();
    private readonly GetCategoriesHandler _handler;

    public GetCategoriesHandlerTests()
    {
        _handler = new GetCategoriesHandler(_repository);
    }

    [Fact]
    public async Task Handle_WithCategories_ReturnsMappedDtos()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category(1, "Work"),
            new Category(2, "Personal"),
            new Category(3, "Shopping")
        };
        _repository.GetAllCategoriesAsync().Returns(categories);

        // Act
        var result = await _handler.Handle(new GetCategoriesQuery(), CancellationToken.None);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Collection(result,
            dto =>
            {
                Assert.Equal(1, dto.Id);
                Assert.Equal("Work", dto.Name);
            },
            dto =>
            {
                Assert.Equal(2, dto.Id);
                Assert.Equal("Personal", dto.Name);
            },
            dto =>
            {
                Assert.Equal(3, dto.Id);
                Assert.Equal("Shopping", dto.Name);
            }
        );
    }

    [Fact]
    public async Task Handle_NoCategories_ReturnsEmptyList()
    {
        // Arrange
        _repository.GetAllCategoriesAsync().Returns(new List<Category>());

        // Act
        var result = await _handler.Handle(new GetCategoriesQuery(), CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }
}
