using OneMore.Domain.Commands.Abstract;

namespace OneMore.Domain.Commands;

using Flunt.Notifications;
using Flunt.Validations;

public class CreateSessionCommand : BaseCommand
{
    public string SessionName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;

    public override bool Validate()
    {
        AddNotifications(new Contract<Notification>()
            .Requires()
            .IsNotNullOrWhiteSpace(SessionName, nameof(SessionName), "O nome da sessão é obrigatório")
            .IsNotNullOrWhiteSpace(UserName, nameof(UserName), "O nome do usuário é obrigatório")
        );

        return base.Validate();
    }
}

public class JoinSessionCommand : BaseCommand
{
    public Guid SessionId { get; set; }
    public string UserName { get; set; } = string.Empty;

    public override bool Validate()
    {
        AddNotifications(new Contract<Notification>()
            .Requires()
            .AreNotEquals(SessionId, Guid.Empty, nameof(SessionId), "Sessão inválida")
            .IsNotNullOrWhiteSpace(UserName, nameof(UserName), "O nome do usuário é obrigatório")
        );

        return base.Validate();
    }
}

public class GetSessionCommand : BaseCommand
{
    public Guid SessionId { get; set; }

    public override bool Validate()
    {
        AddNotifications(new Contract<Notification>()
            .Requires()
            .AreNotEquals(SessionId, Guid.Empty, nameof(SessionId), "Sessão inválida")
        );

        return base.Validate();
    }
}

public class LeaveSessionCommand : BaseCommand
{
    public Guid SessionId { get; set; }
    public string UserName { get; set; } = string.Empty;

    public override bool Validate()
    {
        AddNotifications(new Contract<Notification>()
            .Requires()
            .AreNotEquals(SessionId, Guid.Empty, nameof(SessionId), "Sessão inválida")
            .IsNotNullOrWhiteSpace(UserName, nameof(UserName), "O nome do usuário é obrigatório")
        );

        return base.Validate();
    }
}


public class StartGameCommand : BaseCommand
{
    public Guid SessionId { get; set; }

    public override bool Validate()
    {
        AddNotifications(new Contract<Notification>()
            .Requires()
            .AreNotEquals(SessionId, Guid.Empty, nameof(SessionId), "Sessão inválida")
        );

        return base.Validate();
    }
}



