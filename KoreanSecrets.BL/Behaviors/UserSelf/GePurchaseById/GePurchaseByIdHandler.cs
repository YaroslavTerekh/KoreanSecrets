using AutoMapper;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.UserSelf.GePurchaseById;

public class GePurchaseByIdHandler : IRequestHandler<GePurchaseByIdQuery, PurchaseDTO>
{
    private readonly DataContext _context;
    private readonly UserManager<User> _roleManager;
    private readonly IMapper _mapper;

    public GePurchaseByIdHandler(DataContext context, UserManager<User> roleManager, IMapper mapper)
    {
        _context = context;
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<PurchaseDTO> Handle(GePurchaseByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstAsync(x => x.Id == request.CurrentUserId, cancellationToken: cancellationToken);
        
        if (await CheckAdminRole(user))
        {
            return await _context.Purchases
                .Where(t => t.PurchaseIdentifier == request.Id )
                .Include(x=>x.Products)
                .Include(x=>x.User)
                .Select(t => _mapper.Map<PurchaseDTO>(t))
                .FirstOrDefaultAsync(cancellationToken);
        }
        
        var userPurchases = await _context.Purchases
            .Where(t => t.PurchaseIdentifier == request.Id && t.UserId == request.CurrentUserId)
            .Include(x=>x.Products)
            .Include(x=>x.User)
            .Select(t => _mapper.Map<PurchaseDTO>(t))
            .FirstOrDefaultAsync(cancellationToken);

        return userPurchases;
    }
    
    public async Task<bool> CheckAdminRole(User user)
    {
        return await _roleManager.IsInRoleAsync(user, Roles.Admin);
    }
}
