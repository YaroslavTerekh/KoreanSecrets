using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.MainData.DeleteProduct;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly DataContext _context;
    private readonly IFileService _fileService;

    public DeleteProductHandler(DataContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(t => t.Volumes)
                .ThenInclude(t => t.Photos)
            .Include(t => t.MainPhoto)
            .Include(t => t.Photos)
            .Include(t => t.Guide)
            .FirstOrDefaultAsync(t => t.Id == request.ProductId, cancellationToken: cancellationToken);

        if (product is null)
            throw new NotFoundException(ErrorMessages.SomeProductNotFound);

        var volumeIds = product.Volumes.Select(t => t.Id).ToList();

        var purchasedProducts = await _context.PurchasedProducts.Where(t => volumeIds.Contains(t.VolumeId)).ToListAsync(cancellationToken);
        _context.PurchasedProducts.RemoveRange(purchasedProducts);  
        _context.Products.Remove(product);

        List<AppFile> filesToDelete = new();

        filesToDelete.Add(product.MainPhoto);
        filesToDelete.AddRange(product.Photos);
        filesToDelete.AddRange(product.Volumes.SelectMany(t => t.Photos).ToList());
        filesToDelete.Add(product.Guide);

        foreach (var file in filesToDelete)
        {
            if(file != null)
            {
                await _fileService.DeleteFileAsync2(file.Id, cancellationToken);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
