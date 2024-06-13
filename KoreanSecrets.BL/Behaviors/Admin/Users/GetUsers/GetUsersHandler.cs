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
            .AsQueryable();

        var admins = await _roleManager.GetUsersInRoleAsync(Roles.Admin);
        users = users.Where(t => !admins.Contains(t));

        var usersTotalPurchases = new Dictionary<Guid, long>();

        foreach (var user in users)
            usersTotalPurchases.Add(user.Id, user.Purchases.Select(t => t.TotalPrice).Sum());

        if(request.Text != null)
        {
            users = users.Where(t 
                => String.Concat(t.FirstName, t.LastName).Contains(request.Text)
                || t.PhoneNumber.Contains(request.Text)
                || t.Email.Contains(request.Text));
        }

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
            users = users.OrderByDescending(x => x.CreatedTime);
        }
        
        var res = await users
            .Skip(request.CurrentPage * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var usersResult = res.Select(t => _mapper.Map<UserDTO>(t)).ToList();

        foreach (var user in usersResult)
        {
            user.TotalPurchases = usersTotalPurchases[user.Id];
        }

        return new PaginationModelDTO<UserDTO>
        {
            CurrentPage = request.CurrentPage,
            PageSize = request.PageSize,
            Total = await query.CountAsync(cancellationToken) - admins.Count,
            Products = usersResult
        };
    }

    //public bool CheckAdminRole(User user)
    //{
    //    return _roleManager.IsInRoleAsync(user, Roles.Admin).GetAwaiter().GetResult();
    //}
}
