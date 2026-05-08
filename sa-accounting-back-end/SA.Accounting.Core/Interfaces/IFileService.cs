using Microsoft.AspNetCore.Http;
using SA.Accounting.Core.Entities.Files;

namespace SA.Accounting.Core.Interfaces;

public interface IFileService
{
    Task<List<UploadedFile>> UploadManyAsync(IFormFileCollection formFiles,CancellationToken cancellationToken = default);
    Task<(byte[] fileContent, string contentType, string fileName)> DownloadAsync(Guid id, CancellationToken cancellationToken = default);
}
