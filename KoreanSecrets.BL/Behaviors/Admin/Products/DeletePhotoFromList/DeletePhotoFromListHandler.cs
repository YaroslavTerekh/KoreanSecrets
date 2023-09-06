using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.DeletePhotoFromList;

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
