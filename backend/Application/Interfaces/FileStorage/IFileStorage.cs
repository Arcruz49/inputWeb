namespace InputWeb.Application.Interfaces;

public interface IFileStorage
{
    Task<string> UploadAsync(Stream content, string blobName, string contentType);
    string GenerateDownloadUrl(string blobName, TimeSpan validFor);
}