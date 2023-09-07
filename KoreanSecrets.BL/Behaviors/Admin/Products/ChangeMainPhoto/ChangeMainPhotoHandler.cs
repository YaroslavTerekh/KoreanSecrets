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

namespace KoreanSecrets.BL.Behaviors.Admin.Products.ChangeMainPhoto;

public class ChangeMainPhotoHandler : IRequestHandler<ChangeMainPhotoCommand>
{
    private readonly DataContext _context;
    private readonly IFileService _fileService;

    public ChangeMainPhotoHandler(DataContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<Unit> Handle(ChangeMainPhotoCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(t => t.MainPhoto)
            .Where(t => t.Id == request.ProductId)
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null)
            throw new NotFoundException(ErrorMessages.SomeProductNotFound);

        var oldMainPhoto = product.MainPhoto.Id;

        var newMainPhoto = await _fileService.UploadFileAsync(request.MainPhoto, cancellationToken);
        product.MainPhotoId = newMainPhoto.Id;
        product.MainPhoto = newMainPhoto;
        newMainPhoto.ProductMainPhotoId = product.Id;

        await _context.Files.AddAsync(newMainPhoto, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        await _fileService.DeleteFileAsync(oldMainPhoto, cancellationToken);

        return Unit.Value;
    }
}
