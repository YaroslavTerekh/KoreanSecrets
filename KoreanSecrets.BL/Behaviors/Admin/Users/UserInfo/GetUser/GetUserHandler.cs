using AutoMapper;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Admin.Users.UserInfo.GetUser;

public class GetUserHandler : IRequestHandler<GetUserQuery, UserDTO>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetUserHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDTO> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(x=>x.AddressInfo)
            .Include(t => t.Purchases)
            .Where(x=>x.Id == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (user is null)
        {
            throw new NotFoundException(ErrorMessages.UserNotFound);
        }
        
        var dto = _mapper.Map<UserDTO>(user);

        dto.TotalPurchases = user.Purchases.Select(t => t.TotalPrice).Sum();
        
        return dto;
    }
}
