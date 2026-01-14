using OneMore.Domain.Entities;

namespace OneMore.Api.Hubs.ConnManager;

public interface IConnectionManager
{
    Task Add(Guid sessionId, Guid playerId, string connectionId);
    Task<string?> Get(Guid sessionId, Guid playerId);
    Task RemoveByConnectionId(string connectionId);
}
