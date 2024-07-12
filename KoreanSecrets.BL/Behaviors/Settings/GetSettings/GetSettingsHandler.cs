using KoreanSecrets.BL.Behaviors.Settings.EnableLiqPay;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Settings.GetSettings;

public class GetSettingsHandler : IRequestHandler<GetSettingsCommand, SiteSettings>
{
    private readonly DataContext _context;

    public GetSettingsHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<SiteSettings> Handle(GetSettingsCommand request, CancellationToken cancellationToken)
    {
        var settings = await _context.Settings.FirstOrDefaultAsync(cancellationToken);
        
        return settings;
    }
}
