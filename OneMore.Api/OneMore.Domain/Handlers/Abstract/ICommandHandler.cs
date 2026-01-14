using MediatR;
using OneMore.Domain.Commands.Abstract;

namespace OneMore.Domain.Handlers.Abstract;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, ICommandResult>
    where TCommand : IRequest<ICommandResult>
{
}