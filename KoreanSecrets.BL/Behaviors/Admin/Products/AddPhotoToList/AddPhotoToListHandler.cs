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

namespace KoreanSecrets.BL.Behaviors.Admin.Products.AddPhotoToList;

public class AddPhotoToListHandler : IRequestHandler<AddPhotoToListCommand>
{
    private readonly IFileService _fileService;
    private readonly DataContext _context;

    public AddPhotoToListHandler(IFileService fileService, DataContext context)
    {
        _fileService = fileService;
        _context = context;
    }

    public async Task<Unit> Handle(AddPhotoToListCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(t => t.Photos)
            .FirstOrDefaultAsync(t => t.Id == request.ProductId);

        if (product is null)
            throw new NotFoundException(ErrorMessages.SomeProductNotFound);

        var photo = await _fileService.UploadFileAsync(request.Photo, cancellationToken);
        photo.ProductPhotoId = product.Id;

        await _context.Files.AddAsync(photo, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
