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

namespace KoreanSecrets.BL.Behaviors.UserSelf.ModifyAddressInfo;

public class ModifyAddressInfoHandler : IRequestHandler<ModifyAddressInfoCommand>
{
    private readonly DataContext _context;

    public ModifyAddressInfoHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ModifyAddressInfoCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(t => t.AddressInfo)
            .FirstOrDefaultAsync(t => t.Id == request.CurrentUserId, cancellationToken);

        if(user is null)
            throw new NotFoundException(ErrorMessages.UserNotFound);

        if (user.AddressInfo is null)
        {
            var addressInfo = new AddressInfo
            {
                City = request.City,
                Warehouse = request.Warehouse,
                UserId = user.Id
            };

            await _context.Addresses.AddAsync(addressInfo, cancellationToken);
            user.AddressInfoId = addressInfo.Id;
        } 
        else
        {
            user.AddressInfo.City = request.City;
            user.AddressInfo.Warehouse = request.Warehouse;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
