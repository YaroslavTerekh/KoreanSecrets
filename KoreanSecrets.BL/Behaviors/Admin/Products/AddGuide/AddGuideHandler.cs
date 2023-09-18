using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.AddGuide;

public class AddGuideHandler : IRequestHandler<AddGuideCommand>
{
    private readonly IFileService _fileService;
    private readonly DataContext _context;

    public AddGuideHandler(IFileService fileService, DataContext context)
    {
        _fileService = fileService;
        _context = context;
    }

    public async Task<Unit> Handle(AddGuideCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(t => t.Guide)
            .FirstOrDefaultAsync(t => t.Id == request.ProductId, cancellationToken);

        if (product is null)
            throw new NotFoundException(ErrorMessages.SomeProductNotFound);

        if (product.GuideId is not null)
            await _fileService.DeleteFileAsync(Guid.Parse(product.GuideId.ToString()), cancellationToken);

        var guide = await _fileService.UploadFileAsync(request.File, cancellationToken);
        guide.ProductId = product.Id;
        product.GuideId = guide.Id;

        await _context.Files.AddAsync(guide, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);        

        return Unit.Value;
    }
}
