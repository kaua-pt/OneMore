using OneMore.Domain.Commands;
using Microsoft.AspNetCore.SignalR;

namespace OneMore.Domain.Services.Abstract;

public interface ISessionService
{
    Task CreateSessionAsync(CreateSessionCommand command, HubCallerContext context);
    Task JoinSessionAsync(JoinSessionCommand command, HubCallerContext context);
    Task LeaveSessionAsync(LeaveSessionCommand command, HubCallerContext context);
    Task StartGameAsync(StartGameCommand command, HubCallerContext context);
}
