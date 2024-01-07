using AutoMapper;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Products.GetCategory;

public class GetCategoryHandler : IRequestHandler<GetCategoryQuery, CategoryDTO>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetCategoryHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    //TODO: check-fix
    public async Task<CategoryDTO> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .Where(t => t.Id == request.Id)
            .Include(t => t.Products.Skip(request.CurrentPage * request.PageSize).Take(request.PageSize))
                .ThenInclude(t => t.SubCategory)
            .Include(t => t.Products.Skip(request.CurrentPage * request.PageSize).Take(request.PageSize))
                .ThenInclude(t => t.Country)
            .Include(t => t.Products.Skip(request.CurrentPage * request.PageSize).Take(request.PageSize))
                .ThenInclude(t => t.Demand)
            .Include(t => t.Products.Skip(request.CurrentPage * request.PageSize).Take(request.PageSize))
                .ThenInclude(t => t.Brand)
            .FirstOrDefaultAsync(cancellationToken);

        if (category is null)
            throw new NotFoundException(ErrorMessages.CategoryNotFound);

        var result = new CategoryDTO
        {
            SubCategories = category.Products.Select(t => _mapper.Map<SubCategoryDTO>(t.SubCategory)).ToList(),
            Demands = category.Products.Select(t => _mapper.Map<DemandDTO>(t.Demand)).ToList(),
            Brands = category.Products.Select(t => _mapper.Map<BrandDTO>(t.Brand)).ToList(),
            Products = category.Products.Select(t => _mapper.Map<ListProductDTO>(t)).ToList(),
            Countries = category.Products.Select(t => _mapper.Map<CountryDTO>(t.Country)).ToList(),
            Id = category.Id,
            Title = category.Title,
            CreatedDate = category.CreatedDate
        };

        return result;
    }
}
