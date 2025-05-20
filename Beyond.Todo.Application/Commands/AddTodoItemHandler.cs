using Beyond.Todo.Application.Mapping;
using Beyond.Todo.Application.Services;
using Beyond.Todo.Domain.Entities;
using Beyond.Todo.Infrastructure.Interfaces;
using MediatR;

namespace Beyond.Todo.Application.Commands;

public sealed class AddTodoItemHandler : IRequestHandler<AddTodoItemCommand, int>
{
    private readonly ITodoListRepository _todoRepo;
    private readonly ICategoryRepository _categoryRepo;
    private readonly ITodoNotificationService _notificationService;

    public AddTodoItemHandler(
        ITodoListRepository todoRepo,
        ICategoryRepository categoryRepo,
        ITodoNotificationService notificationService)
    {
        _todoRepo = todoRepo;
        _categoryRepo = categoryRepo;
        _notificationService = notificationService;
    }

    public async Task<int> Handle(AddTodoItemCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepo.GetCategoryByNameAsync(request.Category);
        if (category == null)
            throw new ArgumentException($"[Category '{request.Category}' not found.]");

        int id = _todoRepo.GetNextId();
        var item = new TodoItem(id, request.Title, request.Description, category.Id);
        await _todoRepo.AddAsync(item);
        await _todoRepo.SaveChangesAsync();

        // Notify clients about the new todo item
        var savedItem = await _todoRepo.GetByIdAsync(id);
        await _notificationService.NotifyTodoItemAdded(savedItem.ToDto());

        return id;
    }
}
