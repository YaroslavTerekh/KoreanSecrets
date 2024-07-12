using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Settings.EnableLiqPay;

public class EnableLiqPayHandler : IRequestHandler<EnableLiqPayCommand>
{
    private readonly DataContext _context;

    public EnableLiqPayHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(EnableLiqPayCommand request, CancellationToken cancellationToken)
    {
        var settings = await _context.Settings.FirstOrDefaultAsync(cancellationToken);

        if (settings != null)
        {
            settings.EnableLiqPay = !settings.EnableLiqPay;
            
            _context.Settings.Update(settings);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            settings = new Domain.Entities.SiteSettings();
            await _context.Settings.AddAsync(settings, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
