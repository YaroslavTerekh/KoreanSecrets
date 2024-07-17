using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.UserSelf.SubscribeOnProduct;

public class SubscribeOnProductHandler : IRequestHandler<SubscribeOnProductCommand>
{
    private readonly DataContext _context;

    public SubscribeOnProductHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(SubscribeOnProductCommand request, CancellationToken cancellationToken)
    {
        var volume = await _context.Volume
            .Include(t => t.UsersWaitingForStock)
            .FirstOrDefaultAsync(t => t.Id == request.ProductId, cancellationToken);

        if (volume is null)
            throw new NotFoundException(ErrorMessages.VolumeNotFound);

        var user = await _context.Users
            .FirstOrDefaultAsync(t => t.Id == request.CurrentUserId, cancellationToken);

        if (user is null)
            throw new NotFoundException(ErrorMessages.UserNotFound);

        var newSubscription = new VolumeUser
        {
            UserId = request.CurrentUserId,
            VolumeId = volume.Id
        };

        volume.UsersWaitingForStock.Add(newSubscription);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
