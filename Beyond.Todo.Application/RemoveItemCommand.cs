using MediatR;

namespace Beyond.Todo.Application;

public sealed record RemoveItemCommand(int Id) : IRequest<Unit>;
