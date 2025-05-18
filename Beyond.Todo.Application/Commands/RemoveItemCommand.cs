using MediatR;

namespace Beyond.Todo.Application.Commands;

public sealed record RemoveItemCommand(int Id) : IRequest<Unit>;
