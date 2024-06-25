using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.Volumes.AddNewVolume;

public class AddNewVolumeHandler : IRequestHandler<AddNewVolumeCommand>
{
    private readonly DataContext _context;
    private readonly IFileService _fileService;

    public AddNewVolumeHandler(DataContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<Unit> Handle(AddNewVolumeCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(t => t.Volumes)
            .FirstOrDefaultAsync(t => t.Id == request.ProductId, cancellationToken);

        if (product is null) throw new Exception(ErrorMessages.ProductNotFound("Продукту для модифікації"));

        var volume = new Volume
        {
            ProductId = request.ProductId,
            Price = request.Price,
            Unit = request.Unit,
            Value = request.Value,
            Quantity = request.Quantity
        };

        List<AppFile> photos = new List<AppFile>();

        if (request.Photos != null)
            foreach (var photo in request.Photos)
            {
                var result = await _fileService.UploadFileAsync(photo, cancellationToken);
                result.VolumePhotoId = volume.Id;
                photos.Add(result);
            }

        volume.Photos = photos;

        await _context.Volume.AddAsync(volume, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
