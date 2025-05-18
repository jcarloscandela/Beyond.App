using MediatR;

namespace Beyond.Todo.Application.Commands;

public sealed record RegisterProgressionCommand(int Id, DateTime Date, decimal Percent) : IRequest<Unit>;
