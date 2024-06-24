using KoreanSecrets.BL.Behaviors.Admin.Products.AddPhotoToList;
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

namespace KoreanSecrets.BL.Behaviors.Admin.Products.AddPhotoToVolumeList;

public class AddPhotoToVolumeListHandler : IRequestHandler<AddPhotoToVolumeListCommand>
{
    private readonly IFileService _fileService;
    private readonly DataContext _context;

    public AddPhotoToVolumeListHandler(IFileService fileService, DataContext context)
    {
        _fileService = fileService;
        _context = context;
    }

    public async Task<Unit> Handle(AddPhotoToVolumeListCommand request, CancellationToken cancellationToken)
    {
        var volume = await _context.Volume
            .Include(t => t.Photos)
            .FirstOrDefaultAsync(t => t.Id == request.VolumeId);

        if (volume is null)
            throw new NotFoundException(ErrorMessages.VolumeNotFound);

        var photo = await _fileService.UploadFileAsync(request.Photo, cancellationToken);
        photo.ProductPhotoId = volume.Id;

        await _context.Files.AddAsync(photo, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
