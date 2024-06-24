using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities.Banners;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Admin.BottomBanners.UpdateBottomBannerPhoto;

public class UpdateBottomBannerPhotoHandler : IRequestHandler<UpdateBottomBannerPhotoCommand>
{
    private readonly DataContext _context;
    private readonly IFileService _fileService;

    public UpdateBottomBannerPhotoHandler(DataContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<Unit> Handle(UpdateBottomBannerPhotoCommand request, CancellationToken cancellationToken)
    {
        var banner = await _context.BottomBanners
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == request.BannerId, cancellationToken);

        if (banner is null)
            throw new NotFoundException(ErrorMessages.BannerNotFound);
        
        var photo = await _context.BottomBannerPhotos
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
            x.PhotoId == request.PhotoId && x.BottomBannerId == request.BannerId, 
            cancellationToken: cancellationToken);

        var bottomPhoto = new BottomBannerPhoto();
        
        if (photo is not null)
        {
            bottomPhoto.IsSmall = photo.IsSmall;
            await _fileService.DeleteFileAsync(photo.PhotoId, cancellationToken);
        }

        var newPhoto = await _fileService.UploadFileAsync(request.Photo, cancellationToken);

        bottomPhoto.BottomBannerId = banner.Id;
        bottomPhoto.PhotoId = newPhoto.Id;
        bottomPhoto.Photo = newPhoto;
        
        await  _context.BottomBannerPhotos.AddAsync(bottomPhoto, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
