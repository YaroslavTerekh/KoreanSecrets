using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.DeleteGuide;

public class DeleteGuideHandler : IRequestHandler<DeleteGuideCommand>
{
    private readonly DataContext _context;
    private readonly IFileService _fileService;

    public DeleteGuideHandler(DataContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<Unit> Handle(DeleteGuideCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FirstOrDefaultAsync(t => t.Id == request.ProductId);

        if (product is null)
            throw new NotFoundException(ErrorMessages.SomeProductNotFound);

        product.GuideId = null;

        await _context.SaveChangesAsync(cancellationToken);
        await _fileService.DeleteFileAsync(request.FileId, cancellationToken);

        return Unit.Value;
    }
}
