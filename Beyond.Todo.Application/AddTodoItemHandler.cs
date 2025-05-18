using Beyond.Todo.Domain.Entities;
using Beyond.Todo.Infrastructure;
using Mediator;

namespace Beyond.Todo.Application;

public sealed class AddTodoItemHandler : IRequestHandler<AddTodoItemCommand, int>
{
    private readonly ITodoListRepository _repo;

    public AddTodoItemHandler(ITodoListRepository repo)
    {
        _repo = repo;
    }

    public async ValueTask<int> Handle(AddTodoItemCommand request, CancellationToken cancellationToken)
    {
        var categories = await _repo.GetAllCategoriesAsync();
        if (!categories.Contains(request.Category))
            throw new ArgumentException("Invalid category.");

        int id = _repo.GetNextId();
        var item = new TodoItem(id, request.Title, request.Description, request.Category);
        await _repo.AddAsync(item);
        await _repo.SaveChangesAsync();
        return id;
    }
}
