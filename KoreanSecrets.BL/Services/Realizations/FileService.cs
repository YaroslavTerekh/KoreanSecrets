using Firebase.Auth;
using Firebase.Storage;
using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.Common.Settings;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace KoreanSecrets.BL.Services.Realizations;

public class FileService : IFileService
{
    private readonly DataContext _context;
    private readonly IWebHostEnvironment _env;
    private readonly FirebaseSettings _firebaseSettings;
    public FileService(DataContext context, IWebHostEnvironment env, FirebaseSettings firebaseSettings)
    {
        _context = context;
        _env = env;
        _firebaseSettings = firebaseSettings;
    }

    public async Task<AppFile> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        var extension = Path.GetExtension(file.FileName);
        var fileName = String.Concat(Path.GetFileName(file.FileName), Guid.NewGuid().ToString());

        try
        {
            var newFile = new AppFile
            {
                FileExtension = extension,
                FileName = fileName
            };

            var stream = file.OpenReadStream();

            var auth = new FirebaseAuthProvider(new FirebaseConfig(_firebaseSettings.ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(_firebaseSettings.Email, _firebaseSettings.Password);

            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
                _firebaseSettings.StorageLink,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                })
                .Child("uploads")
                .Child(fileName)
                .PutAsync(stream, cancellation.Token);

            task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

            var videoLink = await task;
            newFile.FilePath = videoLink;

            return newFile;
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task DeleteFileAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var file = await _context.Files.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (file is not null)
        {
            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(_firebaseSettings.ApiKey));
                var a = await auth.SignInWithEmailAndPasswordAsync(_firebaseSettings.Email, _firebaseSettings.Password);

                var cancellation = new CancellationTokenSource();

                var task = new FirebaseStorage(
                    _firebaseSettings.StorageLink,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    })
                    .Child("uploads")
                    .Child(file.FileName)
                    .DeleteAsync();

                await task;
            }
            catch { }
            finally
            {
                _context.Files.Remove(file);
                await _context.SaveChangesAsync();
            }
        }
    }
}
