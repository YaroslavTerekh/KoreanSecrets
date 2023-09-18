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

namespace KoreanSecrets.BL.Behaviors.Admin.Banners.ChangeBannerPhoto;

public class ChangeBannerPhotoHandler : IRequestHandler<ChangeBannerPhotoCommand>
{
    private readonly DataContext _context;
    private readonly IFileService _fileService;

    public ChangeBannerPhotoHandler(DataContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<Unit> Handle(ChangeBannerPhotoCommand request, CancellationToken cancellationToken)
    {
        var banner = await _context.Banners.FirstOrDefaultAsync(t => t.Id == request.BannerId, cancellationToken);

        if (banner is null)
            throw new NotFoundException(ErrorMessages.BannerNotFound);

        await _fileService.DeleteFileAsync(banner.BannerPhotoId);

        var resultFile = await _fileService.UploadFileAsync(request.Photo, cancellationToken);
        resultFile.BannerId = banner.Id;
        banner.BannerPhotoId = resultFile.Id;

        await _context.Files.AddAsync(resultFile, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
