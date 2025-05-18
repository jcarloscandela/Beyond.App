using Mediator;

namespace Beyond.Todo.Application;

public sealed record RemoveItemCommand(int Id) : IRequest;
