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

namespace KoreanSecrets.BL.Behaviors.Admin.Banners.DeleteBanner;

public class DeleteBannerHandler : IRequestHandler<DeleteBannerCommand>
{
    private readonly DataContext _context;
    private readonly IFileService _fileService;

    public DeleteBannerHandler(DataContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<Unit> Handle(DeleteBannerCommand request, CancellationToken cancellationToken)
    {
        var banner = await _context.Banners.FirstOrDefaultAsync(t => t.Id == request.BannerId, cancellationToken);

        if (banner is null)
            throw new NotFoundException(ErrorMessages.BannerNotFound);

        await _fileService.DeleteFileAsync(banner.BannerPhotoId, cancellationToken);

        _context.Banners.Remove(banner);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
