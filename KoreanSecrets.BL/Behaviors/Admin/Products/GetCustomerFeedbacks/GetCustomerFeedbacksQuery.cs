using KoreanSecrets.Domain.DataTransferObjects;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.GetCustomerFeedbacks;

public class GetCustomerFeedbacksQuery : IRequest<PaginationModelDTO<FeedbackDTO>>
{
    public string? Text { get; set; }

    public int CurrentPage { get; set; }

    public int PageSize { get; set; }
}
