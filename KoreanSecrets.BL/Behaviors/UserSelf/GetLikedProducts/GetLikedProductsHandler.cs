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
                .ThenInclude(t => t.Likes)   
                    .ThenInclude(t => t.Volumes)
                        .ThenInclude(x=>x.Photos)
            .Include(t => t.Likes)
                .ThenInclude(t => t.Likes)
                    .ThenInclude(t => t.Brand)
            .Where(t => t.Id == request.CurrentUserId);

        var likes = await _context.Users
            .Skip(request.PageSize * request.CurrentPage)
            .Take(request.PageSize)
            .SelectMany(t => t.Likes.Where(l => l.LikesId1 == request.CurrentUserId))
                .Include(t => t.Likes)
                .ThenInclude(x => x.MainPhoto)
                .Include(t => t.Likes)
                .ThenInclude(x => x.Volumes).ThenInclude(x=>x.Photos)
                .Include(t => t.Likes)
                .ThenInclude(x => x.Brand)
                        .ToListAsync(cancellationToken);

        var mappedLikes = likes
                    .Select(t => _mapper.Map<ListProductDTO>(t.Likes))
                    .ToList();
        foreach (var likedProduct in mappedLikes)
            likedProduct.IsLikedByUser = true;

        return new PaginationModelDTO<ListProductDTO>
        {
            CurrentPage = request.CurrentPage,
            PageSize = request.PageSize,
            Total = await query.Select(t => t.Likes).CountAsync(cancellationToken),
            Products = mappedLikes
        };
    }
}
