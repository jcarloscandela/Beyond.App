using Mediator;

namespace Beyond.Todo.Application;

public sealed record UpdateItemCommand(int Id, string Description) : IRequest;
