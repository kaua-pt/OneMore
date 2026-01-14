using Flunt.Notifications;
using Flunt.Validations;
using MediatR;
using System.Text.Json.Serialization;

namespace OneMore.Domain.Commands.Abstract;

public interface ICommand
{
    bool Validate();
}

public abstract class BaseCommand : Notifiable<Notification>, ICommand, IRequest<ICommandResult>
{
    [JsonIgnore] public DateTime RequestTime { get; private set; } = DateTime.UtcNow;

    public virtual bool Validate()
    {
        AddNotifications(new Contract<BaseCommand>().Requires());
        return IsValid;
    }
}

public abstract class AuthCommand : BaseCommand
{
    [JsonIgnore] public Guid RequestedBy { get; set; } = Guid.Empty;

    public override bool Validate()
    {
        AddNotifications(new Contract<Notification>()
            .Requires()
            .IsNotEmpty(RequestedBy, nameof(RequestedBy), "ID de usuário é obrigatório")
        );
        return base.Validate();
    }
}