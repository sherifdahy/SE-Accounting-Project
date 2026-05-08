using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SA.Accounting.Core.Entities.Files;
using SA.Accounting.Core.Entities.Interfaces;
using SA.Accounting.Core.Interfaces;

namespace SA.Accounting.Services.Services;

public class FileService(IWebHostEnvironment webHostEnvironment,IUnitOfWork unitOfWork) : IFileService
{
    private readonly string _filesPath = $"{webHostEnvironment.WebRootPath}/files";
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<(byte[] fileContent, string contentType, string fileName)> DownloadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var file = await _unitOfWork.Files.FindAsync(x => x.Id == id, [], cancellationToken);

        if (file is null)
            return ([], string.Empty, string.Empty);

        var path = Path.Combine(_filesPath, file.StoredFileName);

        MemoryStream memoryStream = new MemoryStream();

        using FileStream fileStream = new(path, FileMode.Open);
        fileStream.CopyTo(memoryStream);
        memoryStream.Position = 0;

        return (memoryStream.ToArray(), file.ContentType, file.FileName);
    }

    public async Task<List<UploadedFile>> UploadManyAsync(IFormFileCollection formFiles, CancellationToken cancellationToken = default)
    {
        var uploadedFiles = new List<UploadedFile>();

        foreach(var file in formFiles)
        {
            uploadedFiles.Add(await SaveFileAsync(file, cancellationToken));
        }
        return uploadedFiles;
    }

    private async Task<UploadedFile> SaveFileAsync(IFormFile file,CancellationToken cancellationToken = default)
    {
        var randomFileName = Path.GetRandomFileName();

        var uploadedFile = new UploadedFile()
        {
            FileName = file.FileName,
            StoredFileName = randomFileName,
            ContentType = file.ContentType,
            FileExtension = Path.GetExtension(file.FileName)
        };

        var path = Path.Combine(_filesPath, uploadedFile.StoredFileName);

        using Stream stream = File.Create(path);

        await file.CopyToAsync(stream, cancellationToken);

        return uploadedFile;
    }
}
