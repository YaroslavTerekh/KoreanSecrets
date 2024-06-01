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
        var users = query
            .Include(t => t.Purchases)
            .Skip(request.CurrentPage * request.PageSize)
            .Take(request.PageSize)            
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.ColumnToSort))
        {
            users = request.ColumnToSort switch
            {
                "name" => request?.WayToSort == "asc"
                    ? users.OrderBy(t => t.FirstName)
                    : users.OrderByDescending(t => t.FirstName),
                "email" => request?.WayToSort == "asc"
                    ? users.OrderBy(t => t.Email)
                    : users.OrderByDescending(t => t.Email),
                "phone" => request?.WayToSort == "asc"
                    ? users.OrderBy(t => t.PhoneNumber)
                    : users.OrderByDescending(t => t.PhoneNumber),
                "purchases" => request?.WayToSort == "asc"
                    ? users.OrderBy(t => t.Purchases)
                    : users.OrderByDescending(t => t.Purchases),
                _ => users
            };
        }
        else
        {
            users = users.OrderByDescending(x => x.CreatedTime).ThenByDescending(x=>x.Purchases);
        }
        
        var admins = users.Where(t => CheckAdminRole(t)).ToList();
        var res = await users.Where(t => !admins.Contains(t)).ToListAsync(cancellationToken);

        return new PaginationModelDTO<UserDTO>
        {
            CurrentPage = request.CurrentPage,
            PageSize = request.PageSize,
            Total = await query.CountAsync(cancellationToken) - admins.Count,
            Products = res.Select(t => _mapper.Map<UserDTO>(t)).ToList()
        };
    }

    public bool CheckAdminRole(User user)
    {
        return _roleManager.IsInRoleAsync(user, Roles.Admin).GetAwaiter().GetResult();
    }
}
