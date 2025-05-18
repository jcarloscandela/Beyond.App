using MediatR;
using Beyond.Todo.Application.Models;

namespace Beyond.Todo.Application.Queries;

public record GetCategoriesQuery : IRequest<List<CategoryDto>>;
