using MediatR;
using Beyond.Todo.Domain.Entities;
using Beyond.Todo.Infrastructure.Interfaces;

namespace Beyond.Todo.Application;

public record GetCategoriesQuery : IRequest<List<Category>>;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<Category>>
{
    private readonly ICategoryRepository _repository;

    public GetCategoriesQueryHandler(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetAllCategoriesAsync();
    }
}
