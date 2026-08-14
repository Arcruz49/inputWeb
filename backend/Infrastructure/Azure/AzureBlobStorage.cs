using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using InputWeb.Application.Interfaces;

namespace InputWeb.Infrastructure.Storage;

public class AzureBlobStorage : IFileStorage
{
    private readonly BlobContainerClient _container;

    // tenho que validar se isso realmente funciona depois
    // realizar testes para verificar as urls que estão sendo retornadas e os arquivos que estão sendo salvos
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

    public string GenerateDownloadUrl(string blobName, TimeSpan validFor)
    {
        var blobClient = _container.GetBlobClient(blobName);

        if (!blobClient.CanGenerateSasUri)
            throw new InvalidOperationException("Não é possível gerar SAS — verifique a autenticação.");

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = _container.Name,
            BlobName = blobName,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.Add(validFor)
        };
        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        return blobClient.GenerateSasUri(sasBuilder).ToString();
    }
}