using AutoMapper;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.UserSelf.GetLikedProducts;

public class GetLikedProductsHandler : IRequestHandler<GetLikedProductsQuery, PaginationModelDTO<ListProductDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetLikedProductsHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginationModelDTO<ListProductDTO>> Handle(GetLikedProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Users
            .Include(t => t.Likes)
                .ThenInclude(t => t.Brand)
            .Include(t => t.Likes)
                .ThenInclude(t => t.MainPhoto)
            .Where(t => t.Id == request.CurrentUserId);

        var likes = await _context.Users
            .Skip(request.PageSize * request.CurrentPage)
            .Take(request.PageSize)
            .SelectMany(t => t.Likes).Select(t => _mapper.Map<ListProductDTO>(t))
            .ToListAsync(cancellationToken);

        foreach (var likedProduct in likes)
            likedProduct.IsLikedByUser = true;

        return new PaginationModelDTO<ListProductDTO>
        {
            CurrentPage = request.CurrentPage,
            PageSize = request.PageSize,
            Total = await query.Select(t => t.Likes).CountAsync(cancellationToken),
            Products = likes
        };
    }
}
