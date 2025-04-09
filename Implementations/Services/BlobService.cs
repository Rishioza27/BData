using System.ComponentModel;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Wardrobe.Abstractions.Storage;

namespace Wardrobe.Implementations.Services
{
	public class BlobService : IBlobService
	{
		private readonly BlobServiceClient _blobServiceClient;
		private readonly IConfiguration _configuration;

		public BlobService(BlobServiceClient blobServiceClient, IConfiguration configuration)
		{
			_configuration = configuration;
			_blobServiceClient = blobServiceClient;
		}

		public string? GetReadOnlyToken(string ContainerName, string BlobName)
		{
			if (string.IsNullOrEmpty(BlobName))
			{
				return ContainerName;
			}
			BlobClient BlobClient = _blobServiceClient.GetBlobContainerClient(ContainerName).GetBlobClient(BlobName);
			// Create a SAS token that's valid for 1 hours
			BlobSasBuilder sasBuilder = new()
			{
				BlobContainerName = BlobClient.GetParentBlobContainerClient().Name,
				BlobName = BlobClient.Name,
				Resource = "b",
				ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
			};
			sasBuilder.SetPermissions(BlobContainerSasPermissions.Read);

			//Get the sasToken from the query
			Uri SasUri = BlobClient.GenerateSasUri(sasBuilder);
			UriBuilder UpdateSasUri = new(SasUri) { Host = _configuration["StorageCredential:Host"] };
			return UpdateSasUri.Uri.AbsoluteUri.ToString();
		}

		public string? GetUploadOnlyTokenForContainer(string ContainerName, string BlobName)
		{
			// Get a reference to the container
			BlobContainerClient ContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
			BlobClient BlobClient = ContainerClient.GetBlobClient(BlobName);

			// Create a SAS token that's valid for 2 min
			BlobSasBuilder sasBuilder = new()
			{
				BlobContainerName = ContainerClient.Name,
				Resource = "b",
				ExpiresOn = DateTime.UtcNow.AddMinutes(2),
			};
			sasBuilder.SetPermissions(BlobContainerSasPermissions.Write | BlobContainerSasPermissions.Create);

			//Get the sasToken from the query
			Uri SasUri = BlobClient.GenerateSasUri(sasBuilder);
			UriBuilder UpdateSasUri = new(SasUri);
			UpdateSasUri.Host = _configuration["StorageCredential:Host"];
			return UpdateSasUri.Uri.AbsoluteUri.ToString();
		}
	}
}
