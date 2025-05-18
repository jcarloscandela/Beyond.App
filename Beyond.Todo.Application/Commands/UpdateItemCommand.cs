using MediatR;

namespace Beyond.Todo.Application.Commands;

public sealed record UpdateItemCommand(int Id, string Description) : IRequest<Unit>;
