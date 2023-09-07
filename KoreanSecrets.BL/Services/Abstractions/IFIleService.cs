using KoreanSecrets.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Services.Abstractions;

public interface IFileService
{
    public Task<AppFile> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default);

    public Task DeleteFileAsync(Guid id, CancellationToken cancellationToken = default);
}

