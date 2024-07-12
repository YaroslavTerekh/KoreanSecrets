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

namespace KoreanSecrets.BL.Behaviors.UserSelf.SubscribeOnVolumeData.SubscribeOnVolume;

public class SubscribeOnVolumeHandler : IRequestHandler<SubscribeOnVolumeCommand>
{
    private readonly DataContext _context;

    public async Task<Unit> Handle(SubscribeOnVolumeCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(t => t.UsersWaitingForStock)
            .FirstOrDefaultAsync(t => t.Id == request.VolumeId, cancellationToken);

        if (product is null)
            throw new NotFoundException(ErrorMessages.VolumeNotFound);

        var user = await _context.Users
            .FirstOrDefaultAsync(t => t.Id == request.CurrentUserId, cancellationToken);

        if (user is null)
            throw new NotFoundException(ErrorMessages.UserNotFound);
        
        var volumeUser = new VolumeUser
        {
            UserId = request.CurrentUserId,
            VolumeId = request.VolumeId
        };

        await _context.VolumeUser.AddAsync(volumeUser, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;

    }
}
