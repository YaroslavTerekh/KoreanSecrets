using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.Volumes.AddPhotoToVolumeList;

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
        photo.VolumePhotoId = volume.Id;

        await _context.Files.AddAsync(photo, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
