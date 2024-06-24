using KoreanSecrets.BL.Behaviors.Admin.Banners.DeleteBanner;
using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Admin.BottomBanners.DeleteBanner;

public class DeleteBottomBannerHandler : IRequestHandler<DeleteBottomBannerCommand>
{
    private readonly DataContext _context;
    private readonly IFileService _fileService;

    public DeleteBottomBannerHandler(DataContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<Unit> Handle(DeleteBottomBannerCommand request, CancellationToken cancellationToken)
    {
        var banner = await _context.BottomBanners
            .FirstOrDefaultAsync(t => t.Id == request.BannerId, cancellationToken);

        if (banner is null)
            throw new NotFoundException(ErrorMessages.BannerNotFound);
        
        var bannerPhotos = await _context.BottomBannerPhotos
            .Where(x => x.BottomBannerId == request.BannerId)
            .ToListAsync(cancellationToken);

        foreach (var photo in bannerPhotos)
        {
            await _fileService.DeleteFileAsync(photo.PhotoId, cancellationToken);
        }
        
        _context.BottomBanners.Remove(banner);
        
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
