using Mediator;

namespace Beyond.Todo.Application;

public sealed record RegisterProgressionCommand(int Id, DateTime Date, decimal Percent) : IRequest;
