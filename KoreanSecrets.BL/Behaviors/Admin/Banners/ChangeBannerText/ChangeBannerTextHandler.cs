using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Banners.ChangeBannerText;

public class ChangeBannerTextHandler : IRequestHandler<ChangeBannerTextCommand>
{
    private readonly DataContext _context;

    public ChangeBannerTextHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ChangeBannerTextCommand request, CancellationToken cancellationToken)
    {
        var banner = await _context.Banners.FirstOrDefaultAsync(t => t.Id == request.BannerId, cancellationToken);

        if (banner is null)
            throw new NotFoundException(ErrorMessages.BannerNotFound);

        banner.Text = request.Title;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
