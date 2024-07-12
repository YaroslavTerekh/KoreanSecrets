using KoreanSecrets.BL.Behaviors.UserSelf.AddFeedback;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Settings.EnablePayByCard;

public class EnablePayByCardHandler : IRequestHandler<EnablePayByCardCommand>
{
    private readonly DataContext _context;

    public EnablePayByCardHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(EnablePayByCardCommand request, CancellationToken cancellationToken)
    {
        var settings = await _context.Settings.FirstOrDefaultAsync(cancellationToken);

        if (settings != null)
        {
            settings.EnablePayByCard = !settings.EnablePayByCard;
            
            _context.Settings.Update(settings);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            settings = new Domain.Entities.SiteSettings();
            await _context.Settings.AddAsync(settings, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}
