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

namespace KoreanSecrets.BL.Behaviors.Admin.Banners.ChangeBannerOrder;

public class ChangeBannerOrderHandler : IRequestHandler<ChangeBannerOrderCommand>
{
    private readonly DataContext _context;

    public ChangeBannerOrderHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ChangeBannerOrderCommand request, CancellationToken cancellationToken)
    {
        var banner = await _context.Banners.FirstOrDefaultAsync(t => t.Id == request.BannerId, cancellationToken);

        if (banner is null)
            throw new NotFoundException(ErrorMessages.BannerNotFound);

        banner.Order = request.Order;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
