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

namespace KoreanSecrets.BL.Behaviors.Admin.Brands.ChangeBrandPhoto;

public class ChangeBrandPhotoHandler : IRequestHandler<ChangeBrandPhotoCommand>
{
    private readonly DataContext _context;
    private readonly IFileService _fileService;

    public ChangeBrandPhotoHandler(DataContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<Unit> Handle(ChangeBrandPhotoCommand request, CancellationToken cancellationToken)
    {
        var brand = await _context.Brands
            .Include(t => t.Photo)
            .FirstOrDefaultAsync(t => t.Id == request.BrandId, cancellationToken);

        if (brand is null)
            throw new NotFoundException(ErrorMessages.BrandNotFound);

        if (brand.Photo is not null)
        {
            await _fileService.DeleteFileAsync(brand.PhotoId, cancellationToken);
        }

        var photoResult = await _fileService.UploadFileAsync(request.Photo, cancellationToken);
        photoResult.BrandPhotoId = brand.Id;
        brand.PhotoId = photoResult.Id;

        await _context.Files.AddAsync(photoResult, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
