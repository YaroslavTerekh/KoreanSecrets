using AutoMapper;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.Comments.GetComments;

public class GetCommentsHandler : IRequestHandler<GetCommentsQuery, PaginationModelDTO<CommentDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetCommentsHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginationModelDTO<CommentDTO>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        return new PaginationModelDTO<CommentDTO>
        {
            PageSize = request.PageSize,
            CurrentPage = request.CurrentPage,
            Products = await _context.Comment
            .Include(t => t.Replies)
                    .ThenInclude(t => t.Replies)
            .OrderByDescending(t => t.CreatedDate)
            .Skip(request.CurrentPage * request.PageSize)
            .Take(request.PageSize)
            .Select(t => _mapper.Map<CommentDTO>(t))
            .ToListAsync(cancellationToken),
            Total = await _context.Comment.CountAsync(cancellationToken),
    }; 
    }
}
