using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.Common.Enums;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Admin.Brands.DeleteBrand;

public class DeleteBrandHandler : IRequestHandler<DeleteBrandCommand>
{
    private readonly DataContext _context;
    private readonly IFileService _fileService;

    public DeleteBrandHandler(DataContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<Unit> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = await _context.Brands
            .Include(t => t.Products)
            .FirstOrDefaultAsync(t => t.Id == request.BrandId, cancellationToken);

        if (brand is null)
            throw new NotFoundException(ErrorMessages.BrandNotFound);

        await _fileService.DeleteFileAsync(brand.PhotoId, cancellationToken);
        //brand.Products.ForEach(t => t.BrandId = null);
        _context.Brands.Remove(brand);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
