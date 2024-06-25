using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.Volumes.DeleteVolume;

public class DeleteVolumeHandler : IRequestHandler<DeleteVolumeCommand>
{
    private readonly DataContext _context;
    private readonly IFileService _fileService;

    public DeleteVolumeHandler(DataContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<Unit> Handle(DeleteVolumeCommand request, CancellationToken cancellationToken)
    {
        var volume = await _context.Volume
            .Include(volume => volume.Photos)
            .FirstOrDefaultAsync(t => t.Id == request.VolumeId, cancellationToken);

        if (volume is null) throw new Exception(ErrorMessages.ProductNotFound("Об'єкту об'єму для видалення"));

        foreach (var file in volume.Photos)
        {
            if (file != null)
            {
                await _fileService.DeleteFileAsync2(file.Id, cancellationToken);
            }
        }

        _context.Volume.Remove(volume);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
