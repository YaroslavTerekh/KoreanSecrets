using AutoMapper;
using KoreanSecrets.BL.Behaviors.Admin.Products.AddGuide;
using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.GetCustomerFeedbacks;

public class GetCustomerFeedbacksHandler : IRequestHandler<GetCustomerFeedbacksQuery, PaginationModelDTO<FeedbackDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetCustomerFeedbacksHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginationModelDTO<FeedbackDTO>> Handle(GetCustomerFeedbacksQuery request, CancellationToken cancellationToken)
    {
        var feedbacks = _context.Feedbacks
            .Include(x=>x.Product)
            .Include(x=>x.User)
            .Include(x=>x.Replies)
            .ThenInclude(x=>x.User)
            .OrderByDescending(x=>x.CreatedDate)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Text))
        {
            feedbacks = feedbacks.Where(x =>
                (x.FeedbackText.Contains(request.Text) || x.Product.Title.Contains(request.Text)));
        }
        
        return new PaginationModelDTO<FeedbackDTO>
        {
            CurrentPage = request.CurrentPage,
            PageSize = request.PageSize,
            Products = await feedbacks
                .Skip(request.CurrentPage * request.PageSize)
                .Take(request.PageSize)
                .Select(t => _mapper.Map<FeedbackDTO>(t))
                .ToListAsync(cancellationToken),
            Total = await feedbacks.CountAsync(cancellationToken)
        };
    }
}
