using OneMore.Domain.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneMore.Domain.Entities;

public static class SessionStorage {
    public static List<Session> Sessions { get; set; } = [];
    public static Session? GetSessionById(Guid id) =>
        Sessions.Find(session => session.Id == id);
    public static void AddSession(Session session) =>
        Sessions.Add(session);
    public static void RemoveSession(Guid id) =>
        Sessions.RemoveAll(session => session.Id == id);
}

public class Session : Entity
{
    public Session() { }
    public Session(Player masterPlayer, string name)
    {
        Players.Add(masterPlayer);
        MasterId = masterPlayer.Id; 
        Name = name;
    }

    public Guid MasterId { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;    
    public List<Player> Players { get; set; } = [];
    public string WordToGuess { get; set; } = string.Empty;

    public IDictionary<Guid,string> DrawGame(string WordToGuess)
    {
        var impostorId = Players[new Random().Next(Players.Count)].Id;
        return Players.ToDictionary(
            player => player.Id,
            player => player.Id == impostorId ? "Impostor" : WordToGuess);
    }
}
