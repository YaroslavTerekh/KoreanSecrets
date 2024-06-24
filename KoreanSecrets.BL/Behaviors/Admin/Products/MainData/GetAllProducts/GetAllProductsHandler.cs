using AutoMapper;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.MainData.GetAllProducts;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, PaginationModelDTO<PageProductDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetAllProductsHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginationModelDTO<PageProductDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Products
            .AsNoTracking()
            .Include(t => t.Brand)
            .Include(t => t.SubCategory)
            .Include(t => t.Country)
            .Include(t => t.Category)
            .Include(t => t.ProductDemands)
                .ThenInclude(t => t.Demand)
            .Include(t => t.Photos)
            .Include(t => t.Guide)
            .Include(t => t.MainPhoto)
            .Include(t => t.Feedbacks)
                .ThenInclude(t => t.User)
            .Include(t => t.Volumes)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Text))
        {
            query = query.Where(t => t.Title.Contains(request.Text));
        }

        if (!string.IsNullOrEmpty(request.ColumnToSort))
        {
            query = request.ColumnToSort switch
            {
                "name" => request?.WayToSort == "asc"
                    ? query.OrderBy(t => t.Title)
                    : query.OrderByDescending(t => t.Title),
                "category" => request?.WayToSort == "asc"
                    ? query.OrderBy(t => t.Category.Title)
                    : query.OrderByDescending(t => t.Category.Title),
                "country" => request?.WayToSort == "asc"
                    ? query.OrderBy(t => t.Country.Title)
                    : query.OrderByDescending(t => t.Country.Title),
                "demand" => request?.WayToSort == "asc"
                    ? query.OrderBy(t => t.ProductDemands.OrderBy(d => d.Demand.Title))
                    : query.OrderByDescending(t => t.ProductDemands.OrderByDescending(d => d.Demand.Title)),
                "subCategory" => request?.WayToSort == "asc"
                    ? query.OrderBy(t => t.SubCategory.Title)
                    : query.OrderByDescending(t => t.SubCategory.Title),
                "brand" => request?.WayToSort == "asc"
                    ? query.OrderBy(t => t.Brand.Title)
                    : query.OrderByDescending(t => t.Brand.Title),
                "isInStock" => request?.WayToSort == "asc"
                    ? query.OrderBy(t => t.IsInStock)
                    : query.OrderByDescending(t => t.IsInStock),
                _ => query
            };
        }
        else
        {
            query = query.OrderBy(x => x.Brand.Title).ThenByDescending(x => x.CreatedDate);
        }

        
        return new PaginationModelDTO<PageProductDTO>
        {
            CurrentPage = request.CurrentPage,
            PageSize = request.PageSize,
            Total = await query.CountAsync(cancellationToken),
            Products = await query
                .Select(t => _mapper.Map<PageProductDTO>(t))
                .Skip(request.CurrentPage * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken)
        };
    }
}
