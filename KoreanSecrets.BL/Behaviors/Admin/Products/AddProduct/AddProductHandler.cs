using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.AddProduct;

public class AddProductHandler : IRequestHandler<AddProductCommand>
{
    private readonly DataContext _context;
    private readonly IFileService _fileService;

    public AddProductHandler(DataContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<Unit> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Title = request.Title,
            Characteristics = request.Characteristics,
            Syllabes = request.Syllabes,
            Usage = request.Usage,
            Price = request.Price,            
            BrandId = request.BrandId,
            CategoryId = request.CategoryId,
            CountryId = request.CountryId,
            DemandId = request.DemandId,
            SubCategoryId = request.SubCategoryId,
            AdditionalIcon = request.Icon,
            MainPhoto = await _fileService.UploadFileAsync(request.MainPhoto, cancellationToken)
        };

        product.MainPhoto.ProductMainPhotoId = product.Id;
        product.MainPhotoId = product.MainPhoto.Id;
        product.Volumes = request.Volumes.Select(t => new Volume { Unit = t.Unit, Value = t.Value, ProductId = product.Id }).ToList();

        List<AppFile> photos = new List<AppFile>();

        foreach (var photo in request.Photos)
        {
            var result = await _fileService.UploadFileAsync(photo, cancellationToken);
            result.ProductPhotoId = product.Id;
            photos.Add(result);
        }
        product.Photos = photos;

        if (request.VideoGuide is not null)
        {
            var videoResult = await _fileService.UploadFileAsync(request.VideoGuide, cancellationToken);
            videoResult.ProductId = product.Id;
            product.Guide = videoResult;
            product.GuideId = videoResult.Id;
        }

        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
