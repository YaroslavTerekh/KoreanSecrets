using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Admin.BottomBanners.ChangeOrderBanner;

public class ChangeOrderBottomBannerHandler : IRequestHandler<ChangeOrderBottomBannerCommand>
{
    private readonly DataContext _context;
    private readonly IFileService _fileService;

    public ChangeOrderBottomBannerHandler(DataContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<Unit> Handle(ChangeOrderBottomBannerCommand request, CancellationToken cancellationToken)
    {
        var banner = await _context.BottomBanners.FirstOrDefaultAsync(t => t.Id == request.BannerId, cancellationToken);

        if (banner is null)
            throw new NotFoundException(ErrorMessages.BannerNotFound);

        banner.Order = request.Order;
        
        _context.BottomBanners.Update(banner);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
