using MediatR;
using Beyond.Todo.Infrastructure;

namespace Beyond.Todo.Application;

public record GetCategoriesQuery : IRequest<List<string>>;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<string>>
{
    private readonly ITodoListRepository _repository;

    public GetCategoriesQueryHandler(ITodoListRepository repository)
    {
        _repository = repository;
    }

    public Task<List<string>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetAllCategoriesAsync();
    }
}
