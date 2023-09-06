﻿using AutoMapper;
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

namespace KoreanSecrets.BL.Behaviors.Products.GetProducts;

public class GetProductsHandler : IRequestHandler<GetProductsQuery, PaginnationModelDTO<ListProductDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetProductsHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginnationModelDTO<ListProductDTO>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Products
            .AsNoTracking()
            .Include(t => t.Brand)
            .Include(t => t.MainPhoto)
            .Where(t => t.CategoryId == request.CategoryId);

        if (request.CountriesIds.Count > 0) query = query.Where(t => request.CountriesIds.Contains(t.CountryId));
        if (request.SubCategoriesIds.Count > 0) query = query.Where(t => request.SubCategoriesIds.Contains(t.SubCategoryId));
        if (request.DemandsIds.Count > 0) query = query.Where(t => request.DemandsIds.Contains(t.DemandId));
        if (request.BrandsIds.Count > 0) query = query.Where(t => request.BrandsIds.Contains(t.BrandId));

        return new PaginnationModelDTO<ListProductDTO>
        {
            CurrentPage = request.CurrentPage,
            PageSize = request.PageSize,
            Total = await query.CountAsync(cancellationToken),
            Products = await query
                .Skip(request.CurrentPage * request.PageSize)
                .Take(request.PageSize)
                .Select(t => _mapper.Map<ListProductDTO>(t))
                .ToListAsync(cancellationToken)
    };
    }
}