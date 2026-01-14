using Microsoft.AspNetCore.SignalR;
using OneMore.Api.Hubs.ConnManager;
using OneMore.Domain.Commands;
using OneMore.Domain.Entities;
using OneMore.Domain.Handlers;
using OneMore.Domain.Services.Abstract;

namespace OneMore.Domain.Services;

public class SessionService(
    SessionHandler handler,
    IConnectionManager connectionManager,
    IHubContext<SessionHub> hubContext
) : ISessionService
{
    private readonly IHubContext<SessionHub> _hubContext = hubContext;
    private readonly IConnectionManager _connectionManager = connectionManager;
    private readonly SessionHandler _handler = handler;

    public async Task CreateSessionAsync(CreateSessionCommand command, HubCallerContext context)
    {
        try
        {
            var session = await _handler.Handle(command, CancellationToken.None);
            if (session is null)
            {
                await SendError(context, "CreateSession", new Exception("Session creation failed"));
                return;
            }

            await _connectionManager.Add(session!.Id, session.MasterId, context.ConnectionId);
            await _hubContext.Groups.AddToGroupAsync(context.ConnectionId, session.Id.ToString());

            await _hubContext.Clients
                .Client(context.ConnectionId)
                .SendAsync("SessionCreated", session);
        }
        catch (Exception ex)
        {
            await SendError(context, "CreateSession", ex);
        }
    }

    public async Task JoinSessionAsync(JoinSessionCommand command, HubCallerContext context)
    {
        try
        {
            var player = await _handler.Handle(command, CancellationToken.None);
            if(player is null)
            {
                await SendError(context, "JoinSession", new Exception("Joining session failed"));
                return;
            }
                
            await _connectionManager.Add(command.SessionId, player.Id, context.ConnectionId);
            await _hubContext.Groups.AddToGroupAsync(context.ConnectionId, command.SessionId.ToString());

            var session =
                await _handler.Handle(
                    new GetSessionCommand { SessionId = command.SessionId },
                    CancellationToken.None);

            await _hubContext.Clients
                .Client(context.ConnectionId)
                .SendAsync("SessionJoined", session);

            await _hubContext.Clients
                .Group(command.SessionId.ToString())
                .SendAsync("PlayerJoined", player.Name);
        }
        catch (Exception ex)
        {
            await SendError(context, "JoinSession", ex);
        }
    }

    public async Task LeaveSessionAsync(LeaveSessionCommand command, HubCallerContext context)
    {
        try
        {
            await _handler.Handle(command, CancellationToken.None);

            await _hubContext.Groups.RemoveFromGroupAsync(
                context.ConnectionId,
                command.SessionId.ToString());

            await _connectionManager.RemoveByConnectionId(context.ConnectionId);

            await _hubContext.Clients
                .Group(command.SessionId.ToString())
                .SendAsync("UserLeft", command.UserName);
        }
        catch (Exception ex)
        {
            await SendError(context, "LeaveSession", ex);
        }
    }

    public async Task StartGameAsync(StartGameCommand command, HubCallerContext context)
    {
        try
        {
            var updates = await _handler.Handle(command, CancellationToken.None);
            if (updates is null)
            {
                await SendError(context, "StartGame", new Exception("Start game failed"));
                return;
            }

            await _hubContext.Clients
                .Group(command.SessionId.ToString())
                .SendAsync("GameStarted", command.SessionId);

            await Task.WhenAll(
                updates.Select(async player =>
                {
                    var connectionId =
                        await _connectionManager.Get(command.SessionId, player.Key);

                    if (connectionId is null)
                        return;

                    await _hubContext.Clients
                        .Client(connectionId)
                        .SendAsync("Word", player.Value);
                })
            );
        }
        catch (Exception ex)
        {
            await SendError(context, "StartGame", ex);
        }
    }

    private async Task SendError(
        HubCallerContext context,
        string action,
        Exception ex)
    {
        await _hubContext.Clients
            .Client(context.ConnectionId)
            .SendAsync("Error", new
            {
                action,
                message = ex.Message
            });
    }
}
