using KoreanSecrets.BL.Behaviors.Admin.Banners.AddBanner;
using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using KoreanSecrets.Domain.Entities.Banners;
using MediatR;

namespace KoreanSecrets.BL.Behaviors.Admin.BottomBanners.AddBanner;

public class AddBottomBannerHandler : IRequestHandler<AddBottomBannerCommand>
{
    private readonly DataContext _context;
    private readonly IFileService _fileService;

    public AddBottomBannerHandler(DataContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<Unit> Handle(AddBottomBannerCommand request, CancellationToken cancellationToken)
    {
        var banner = new BottomBanner
        {
            Order = request.Order
        };

        var resultFile = await _fileService.UploadFileAsync(request.FirstPhoto, cancellationToken);
        var resultSecondFile = await _fileService.UploadFileAsync(request.SecondPhoto, cancellationToken);
        
        banner.Photos = new List<BottomBannerPhoto>()
        {
            new BottomBannerPhoto()
            {
                BottomBanner = banner,
                Photo = resultFile,
                PhotoId = resultFile.Id
            },
            new BottomBannerPhoto()
            {
                BottomBanner = banner,
                Photo = resultSecondFile,
                PhotoId = resultSecondFile.Id,
                IsSmall = true
            }
        };
        
        await _context.Files.AddAsync(resultFile, cancellationToken);
        await _context.Files.AddAsync(resultSecondFile, cancellationToken);
        
        await _context.BottomBanners.AddAsync(banner, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
