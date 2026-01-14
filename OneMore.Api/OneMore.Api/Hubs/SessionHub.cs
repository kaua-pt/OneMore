using OneMore.Domain.Commands;
using OneMore.Domain.Services.Abstract;

public class SessionHub : Microsoft.AspNetCore.SignalR.Hub
{
    private readonly ISessionService _realtime;

    public SessionHub(ISessionService realtime) => _realtime = realtime;

    public Task CreateSession(CreateSessionCommand command)
        => _realtime.CreateSessionAsync(command, Context);

    public Task JoinSession(JoinSessionCommand command)
        => _realtime.JoinSessionAsync(command, Context);

    public Task LeaveSession(LeaveSessionCommand command)
        => _realtime.LeaveSessionAsync(command, Context);

    public Task StartGame(StartGameCommand command)
        => _realtime.StartGameAsync(command, Context);
}
