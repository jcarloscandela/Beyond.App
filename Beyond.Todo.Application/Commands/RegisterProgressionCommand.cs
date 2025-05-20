using MediatR;

namespace Beyond.Todo.Application.Commands;

public sealed record RegisterProgressionCommand(int Id, DateTime Date, int Percent) : IRequest<Unit>;
