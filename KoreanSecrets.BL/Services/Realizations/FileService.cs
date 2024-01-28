using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Services.Realizations;

public class FileService : IFileService
{
    private readonly DataContext _context;
    private readonly IWebHostEnvironment _env;

    public FileService(DataContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<AppFile> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        var extension = Path.GetExtension(file.FileName);

        var fileName = Path.GetFileName(file.FileName);
        var filePathName = fileName + "_" + Guid.NewGuid() + extension;
        var path = Path.Combine("uploads", filePathName);
        var uploadPath = Path.Combine(_env.ContentRootPath, "uploads", filePathName);

        try
        {
            var newFile = new AppFile
            {
                FilePath = path,
                FileExtension = extension,
                FileName = fileName
            };

            using (var fs = new FileStream(uploadPath, FileMode.CreateNew))
            {
                await file.CopyToAsync(fs, cancellationToken);
            }

            return newFile;
        }
        catch (Exception e)
        {
            File.Delete(uploadPath);
            throw;
        }
    }

    public async Task DeleteFileAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var file = await _context.Files.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (file is not null)
        {
            var path = Path.Combine(_env.ContentRootPath, file.FilePath);

            _context.Files.Remove(file);
            await _context.SaveChangesAsync(cancellationToken);

            File.Delete(path);
        }
    }
}
