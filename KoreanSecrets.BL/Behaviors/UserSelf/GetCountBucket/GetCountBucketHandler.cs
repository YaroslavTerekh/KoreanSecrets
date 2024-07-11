using AutoMapper;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.UserSelf.GetCountBucket;

public class GetCountBucketHandler : IRequestHandler<GetCountBucketQuery, int>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetCountBucketHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(GetCountBucketQuery request, CancellationToken cancellationToken)
    {
        var bucket = await _context.Users
            .Include(t => t.Bucket)
                .ThenInclude(t => t.BucketProducts)
            .Where(t => t.Id == request.CurrentUserId)
        
            .FirstOrDefaultAsync(cancellationToken);

        return bucket?.Bucket?.BucketProducts?.Count ?? 0;
    }
}
