using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Brands.AddBrand;

public class AddBrandHandler : IRequestHandler<AddBrandCommand>
{
    private readonly DataContext _context;
    private readonly IFileService _fileService;

    public AddBrandHandler(DataContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    //TODO: check-fix
    public async Task<Unit> Handle(AddBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = new Brand
        {
            Title = request.Title
        };

        var photoResult = await _fileService.UploadFileAsync(request.Photo, cancellationToken);
        photoResult.BrandPhotoId = brand.Id;
        brand.PhotoId = photoResult.Id;

        await _context.Files.AddAsync(photoResult, cancellationToken);
        await _context.Brands.AddAsync(brand, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
