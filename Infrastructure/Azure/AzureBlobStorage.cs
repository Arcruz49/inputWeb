using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using InputWeb.Application.Interfaces;

namespace InputWeb.Infrastructure.Storage;

public class AzureBlobStorage : IFileStorage
{
    private readonly BlobContainerClient _container;

    public AzureBlobStorage(IConfiguration config)
    {
        var connectionString = config.GetConnectionString("BlobStorage")
            ?? throw new Exception("BlobStorage connection string not configured");
        var containerName = config["BlobStorage:ContainerName"] ?? "recordings";

        _container = new BlobContainerClient(connectionString, containerName);
        _container.CreateIfNotExists(PublicAccessType.None); // container privado
    }

    public async Task<string> UploadAsync(Stream content, string blobName, string contentType)
    {
        var blob = _container.GetBlobClient(blobName);
        await blob.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType });
        return blob.Uri.ToString();
    }
}