using KoreanSecrets.Domain.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.Comments.GetComments;

public class GetCommentsQuery : IRequest<PaginationModelDTO<CommentDTO>>
{
    public int CurrentPage { get; set; }

    public int PageSize { get; set; }
}
