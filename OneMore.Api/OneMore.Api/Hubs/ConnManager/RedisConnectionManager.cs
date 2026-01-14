using StackExchange.Redis;

namespace OneMore.Api.Hubs.ConnManager;

public class RedisConnectionManager : IConnectionManager
{
    private readonly IDatabase _db;

    public RedisConnectionManager(IConnectionMultiplexer redis) => _db = redis.GetDatabase();

    public async Task Add(Guid sessionId, Guid playerId, string connectionId)
    {
        var sessionKey = $"session:{sessionId}:player:{playerId}";
        var reverseKey = $"connection:{connectionId}";

        await _db.StringSetAsync(sessionKey, connectionId);
        await _db.StringSetAsync(reverseKey, $"{sessionId}|{playerId}");
    }

    public async Task<string?> Get(Guid sessionId, Guid playerId)
    {
        return await _db.StringGetAsync($"session:{sessionId}:player:{playerId}");
    }

    public async Task RemoveByConnectionId(string connectionId)
    {
        var reverseKey = $"connection:{connectionId}";
        var value = await _db.StringGetAsync(reverseKey);

        if (!value.HasValue) return;

        var parts = value.ToString().Split('|');
        var sessionId = parts[0];
        var playerId = parts[1];

        await _db.KeyDeleteAsync(reverseKey);
        await _db.KeyDeleteAsync($"session:{sessionId}:player:{playerId}");
    }
}
