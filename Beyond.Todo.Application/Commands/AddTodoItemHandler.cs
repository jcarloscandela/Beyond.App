using Beyond.Todo.Domain.Exceptions;
using Beyond.Todo.Domain.Entities;
using Beyond.Todo.Infrastructure.Interfaces;
using MediatR;

namespace Beyond.Todo.Application.Commands;

public sealed class AddTodoItemHandler : IRequestHandler<AddTodoItemCommand, int>
{
    private readonly ITodoListRepository _todoRepo;
    private readonly ICategoryRepository _categoryRepo;

    public AddTodoItemHandler(ITodoListRepository todoRepo, ICategoryRepository categoryRepo)
    {
        _todoRepo = todoRepo;
        _categoryRepo = categoryRepo;
    }

    public async Task<int> Handle(AddTodoItemCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepo.GetCategoryByNameAsync(request.Category);
        if (category == null)
            throw new TodoException($"Category '{request.Category}' not found.");

        int id = _todoRepo.GetNextId();
        var item = new TodoItem(id, request.Title, request.Description, category.Id);
        await _todoRepo.AddAsync(item);
        await _todoRepo.SaveChangesAsync();
        return id;
    }
}
