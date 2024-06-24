using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.Volumes.AddNewVolumes;

public class AddNewVolumesHandler : IRequestHandler<AddNewVolumesCommand>
{
    private readonly DataContext _context;
    private readonly IFileService _fileService;

    public AddNewVolumesHandler(DataContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<Unit> Handle(AddNewVolumesCommand request, CancellationToken cancellationToken)
    {
        var list = new List<Volume>();
        
        foreach (var item in request.Data)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(t => t.Id == item.ProductId, cancellationToken);

            if (product is null) throw new Exception(ErrorMessages.ProductNotFound("Продукту для модифікації"));

            var volume = new Volume
            {
                ProductId = item.ProductId,
                Price = item.Price,
                Unit = item.Unit,
                Value = item.Value,
                Quantity = item.Quantity
            };

            List<AppFile> photos = new List<AppFile>();

            if (item.Photos != null)
                foreach (var photo in item.Photos)
                {
                    var result = await _fileService.UploadFileAsync(photo, cancellationToken);
                    result.VolumePhotoId = volume.Id;
                    photos.Add(result);
                }

            volume.Photos = photos;
            
            list.Add(volume);
        }

        await _context.Volume.AddRangeAsync(list, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
