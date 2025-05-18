using MediatR;

namespace Beyond.Todo.Application;

public sealed record UpdateItemCommand(int Id, string Description) : IRequest<Unit>;
