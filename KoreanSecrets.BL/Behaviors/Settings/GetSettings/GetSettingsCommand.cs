using KoreanSecrets.Domain.Entities;
using MediatR;

namespace KoreanSecrets.BL.Behaviors.Settings.GetSettings;

public class GetSettingsCommand : IRequest<SiteSettings>
{
}
