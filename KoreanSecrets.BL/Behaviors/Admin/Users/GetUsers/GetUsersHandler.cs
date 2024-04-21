using AutoMapper;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Users.GetUsers;

public class GetUsersHandler : IRequestHandler<GetUsersQuery, PaginationModelDTO<UserDTO>>
{
    private readonly DataContext _context;
    private readonly UserManager<User> _roleManager;
    private readonly IMapper _mapper;

    public GetUsersHandler(DataContext context, IMapper mapper, UserManager<User> roleManager)
    {
        _context = context;
        _mapper = mapper;
        _roleManager = roleManager;
    }

    public async Task<PaginationModelDTO<UserDTO>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Users.AsQueryable();
        var users = await query
            .Skip(request.CurrentPage * request.PageSize)
            .Take(request.PageSize)            
            .ToListAsync(cancellationToken);

        var admins = users.Where(t => CheckAdminRole(t)).ToList();
        users = users.Where(t => !admins.Contains(t)).ToList();

        return new PaginationModelDTO<UserDTO>
        {
            CurrentPage = request.CurrentPage,
            PageSize = request.PageSize,
            Total = await query.CountAsync(cancellationToken),
            Products = users.Select(t => _mapper.Map<UserDTO>(t)).ToList()
        };
    }

    public bool CheckAdminRole(User user)
    {
        return _roleManager.IsInRoleAsync(user, Roles.Admin).GetAwaiter().GetResult();
    }
}
