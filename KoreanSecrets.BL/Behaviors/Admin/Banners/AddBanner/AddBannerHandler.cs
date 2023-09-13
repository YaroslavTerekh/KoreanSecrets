using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Banners.AddBanner;

public class AddBannerHandler : IRequestHandler<AddBannerCommand>
{
    private readonly DataContext _context;
    private readonly IFileService _fileService;

    public AddBannerHandler(DataContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<Unit> Handle(AddBannerCommand request, CancellationToken cancellationToken)
    {
        var banner = new Banner
        {
            Text = request.Title,
            ProductId = request.ProductId
        };

        var resultFile = await _fileService.UploadFileAsync(request.Photo, cancellationToken);
        resultFile.BannerId = banner.Id;
        banner.BannerPhotoId = resultFile.Id;

        await _context.Files.AddAsync(resultFile, cancellationToken);
        await _context.Banners.AddAsync(banner, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
