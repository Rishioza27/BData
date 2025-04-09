namespace Wardrobe.Abstractions.Storage
{
	public interface IBlobService
	{
		string? GetReadOnlyToken(string ContainerName, string BlobName);
		string? GetUploadOnlyTokenForContainer(string ContainerName, string BlobName);
	}
}
