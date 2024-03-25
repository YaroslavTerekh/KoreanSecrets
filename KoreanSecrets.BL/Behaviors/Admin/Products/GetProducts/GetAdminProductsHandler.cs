﻿using AutoMapper;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.GetProducts;

public class GetAdminProductsHandler : IRequestHandler<GetAdminProductsQuery, PaginationModelDTO<PageProductDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetAdminProductsHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginationModelDTO<PageProductDTO>> Handle(GetAdminProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Products
            .Include(t => t.Category)
            .Include(t => t.Country)
            .Include(t => t.Demand)
            .Include(t => t.Brand)
            .Include(t => t.SubCategory)
            .Where(t => t.Title.Contains(request.SearchText));

        return new PaginationModelDTO<PageProductDTO>
        {
            CurrentPage = request.CurrentPage,
            PageSize = request.PageSize,
            Products = await query
                .Skip(request.CurrentPage * request.PageSize)
                .Take(request.PageSize)
                .Select(t => _mapper.Map<PageProductDTO>(t))
                .ToListAsync(cancellationToken),
            Total = await query.CountAsync(cancellationToken)
        };
    }
}