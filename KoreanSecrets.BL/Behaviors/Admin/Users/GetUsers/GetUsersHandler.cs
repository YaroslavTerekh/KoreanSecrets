using AutoMapper;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
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
    private readonly IMapper _mapper;

    public GetUsersHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginationModelDTO<UserDTO>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Users.AsQueryable();
        var users = await query
            .Skip(request.CurrentPage * request.PageSize)
            .Take(request.PageSize)
            .Select(t => _mapper.Map<UserDTO>(t))
            .ToListAsync(cancellationToken);

        return new PaginationModelDTO<UserDTO>
        {
            CurrentPage = request.CurrentPage,
            PageSize = request.PageSize,
            Total = await query.CountAsync(cancellationToken),
            Products = users
        };
    }
}
