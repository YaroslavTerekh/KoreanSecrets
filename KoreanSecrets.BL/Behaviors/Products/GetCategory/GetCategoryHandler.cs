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

    public async Task<CategoryDTO> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .Where(t => t.Id == request.Id)
            .Include(t => t.Products.Skip(request.CurrentPage * request.PageSize).Take(request.PageSize))
            .Include(t => t.Brands)
            .Include(t => t.SubCategories)
            .Include(t => t.Countries)
            .Include(t => t.Demands)
            .Select(t => _mapper.Map<CategoryDTO>(t))
            .FirstOrDefaultAsync(cancellationToken);

        if (category is null)
            throw new NotFoundException(ErrorMessages.CategoryNotFound);

        return category;
    }
}
