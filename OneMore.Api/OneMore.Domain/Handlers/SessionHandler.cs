using OneMore.Domain.Commands;
using OneMore.Domain.Entities;
using OneMore.Domain.Repositories;

namespace OneMore.Domain.Handlers;

public class SessionHandler(IWordRepository wordRepository)
{
    private readonly IWordRepository _wordRepository = wordRepository;
    public async Task<Session?> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!request.Validate())
                return null;

            var Player = new Player()
            {
                Name = request.UserName
            };

            var Session = new Session(Player, request.SessionName);
            SessionStorage.AddSession(Session);

            return Session;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }

    }

    public async Task<Player?> Handle(JoinSessionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!request.Validate())
                return null;

            var Player = new Player()
            {
                Name = request.UserName
            };

            var Session = SessionStorage.GetSessionById(request.SessionId);
            if (Session == null)
                return null;

            Session.Players.Add(Player);

            Player.Session = Session;

            return Player;

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }

    }


    public async Task<Session?> Handle(GetSessionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!request.Validate())
                return null;

            var Session = SessionStorage.GetSessionById(request.SessionId);
            if (Session == null)
                return null;
            return Session;

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }

    }

    public async Task<Session?> Handle(LeaveSessionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!request.Validate())
                return null;

            var Session = SessionStorage.GetSessionById(request.SessionId);
            if (Session == null)
                return null;

            Session.Players.RemoveAll(player => player.Name == request.UserName);

            return Session;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<IDictionary<Guid, string>?> Handle(StartGameCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!request.Validate())
                return null;

            var Session = SessionStorage.GetSessionById(request.SessionId);
            if (Session == null)
                return null;

            var Word = await _wordRepository.GetRandomWordAsync();

            return Session.DrawGame(Word);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}
