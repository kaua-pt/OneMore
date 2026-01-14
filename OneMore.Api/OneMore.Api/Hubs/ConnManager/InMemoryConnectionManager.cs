using System.Collections.Concurrent;

namespace OneMore.Api.Hubs.ConnManager;

public class InMemoryConnectionManager : IConnectionManager
{
    private static readonly ConcurrentDictionary<Guid, ConcurrentDictionary<Guid, string>> _sessions = new();

    private static readonly ConcurrentDictionary<string, (Guid sessionId, Guid playerId)> _reverse = new();

    public Task Add(Guid sessionId, Guid playerId, string connectionId)
    {
        var players = _sessions.GetOrAdd(sessionId, _ => new ConcurrentDictionary<Guid, string>());
        players[playerId] = connectionId;

        _reverse[connectionId] = (sessionId, playerId);

        return Task.CompletedTask;
    }

    public Task<string?> Get(Guid sessionId, Guid playerId)
    {
        if (_sessions.TryGetValue(sessionId, out var players) &&
            players.TryGetValue(playerId, out var connectionId))
        {
            return Task.FromResult<string?>(connectionId);
        }

        return Task.FromResult<string?>(null);
    }

    public Task RemoveByConnectionId(string connectionId)
    {
        if (_reverse.TryRemove(connectionId, out var info))
        {
            if (_sessions.TryGetValue(info.sessionId, out var players))
            {
                players.TryRemove(info.playerId, out _);

                if (players.IsEmpty)
                    _sessions.TryRemove(info.sessionId, out _);
            }
        }

        return Task.CompletedTask;
    }
}
