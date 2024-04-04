using AutoMapper;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.UserSelf.GetUser;

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
        var user =  await _context.Users
            .Include(t => t.AddressInfo)
            .FirstOrDefaultAsync(t => t.Id == request.CurrentUserId, cancellationToken);

        return  _mapper.Map<UserDTO>(user);
    }
}
