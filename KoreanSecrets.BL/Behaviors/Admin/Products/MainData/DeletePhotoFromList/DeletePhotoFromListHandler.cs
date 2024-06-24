using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.DbConnection;
using MediatR;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.MainData.DeletePhotoFromList;

public class DeletePhotoFromListHandler : IRequestHandler<DeletePhotoFromListCommand>
{
    private readonly DataContext _context;
    private readonly IFileService _fileService;

    public DeletePhotoFromListHandler(DataContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<Unit> Handle(DeletePhotoFromListCommand request, CancellationToken cancellationToken)
    {
        await _fileService.DeleteFileAsync(request.PhotoId, cancellationToken);

        return Unit.Value;
    }
}
