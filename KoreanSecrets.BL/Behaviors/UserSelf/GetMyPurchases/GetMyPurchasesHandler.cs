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

namespace KoreanSecrets.BL.Behaviors.UserSelf.GetMyPurchases;

public class GetMyPurchasesHandler : IRequestHandler<GetMyPurchasesQuery, PaginationModelDTO<PurchaseDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetMyPurchasesHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginationModelDTO<PurchaseDTO>> Handle(GetMyPurchasesQuery request, CancellationToken cancellationToken)
    {
        var userPurchases = _context.Purchases.Where(t => t.UserId == request.CurrentUserId);

        var result = await userPurchases
            .Include(x=>x.Products)
            .OrderByDescending(x=>x.CreatedDate)
            .Skip(request.PageSize * request.CurrentPage)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);


        return new PaginationModelDTO<PurchaseDTO>()
        {
            CurrentPage = request.CurrentPage,
            PageSize = request.PageSize,
            Products = result.Select(t => _mapper.Map<PurchaseDTO>(t)).ToList(),
            Total = await userPurchases.CountAsync(cancellationToken)
        };
    }
}
