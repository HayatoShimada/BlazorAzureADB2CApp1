using Azure.Identity;
using System.Text;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace BlazorAzureADB2CApp1.Helpers
{
    public static class StorageHelper
    {
        private static IConfiguration Configuration { get; set; }

        static StorageHelper()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        static public async Task UploadBlob(string blobName, string blobContents)
        {
            var accountName = Configuration["AzureStorageConfig:AccountName"];
            var containerName = Configuration["AzureStorageConfig:ContainerName"];
            var clientId = Configuration["AzureStorageConfig:ClientId"];

            string containerEndPoint = string.Format("https://{0}.blob.core.windows.net/{1}", accountName, containerName);

            BlobContainerClient containerClient = new BlobContainerClient(new Uri(containerEndPoint),
                                                                          new ManagedIdentityCredential(clientId));

            try
            {
                // Create the container if it does not exist.
                await containerClient.CreateIfNotExistsAsync();

                // Upload text to a new block blob.
                byte[] byteArray = Encoding.ASCII.GetBytes(blobContents);

                using (MemoryStream stream = new MemoryStream(byteArray))
                {
                    await containerClient.UploadBlobAsync(blobName, stream);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        static public async Task DeleteBlob(string blobName)
        {

            var accountName = Configuration["AzureStorageConfig:AccountName"];
            var containerName = Configuration["AzureStorageConfig:ContainerName"];
            var clientId = Configuration["AzureStorageConfig:ClientId"];

            string containerEndPoint = string.Format("https://{0}.blob.core.windows.net/{1}", accountName, containerName);

            BlobContainerClient containerClient = new BlobContainerClient(new Uri(containerEndPoint),
                                                                          new ManagedIdentityCredential(clientId));

            try
            {

                var blob = containerClient.GetBlobClient(blobName);
                blob.DeleteIfExists();
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        static public async Task<List<CommentBlobDTO>> GetBlobs()
        {
            List<CommentBlobDTO> blobs = new List<CommentBlobDTO>();

            var accountName = Configuration["AzureStorageConfig:AccountName"];
            var containerName = Configuration["AzureStorageConfig:ContainerName"];
            var clientId = Configuration["AzureStorageConfig:ClientId"];

            string containerEndPoint = string.Format("https://{0}.blob.core.windows.net/{1}", accountName, containerName);

            BlobContainerClient containerClient = new BlobContainerClient(new Uri(containerEndPoint),
                                                                          new ManagedIdentityCredential(clientId));

            await containerClient.CreateIfNotExistsAsync();

            try
            {
                // List all the blobs                
                await foreach (BlobItem blob in containerClient.GetBlobsAsync())
                {
                    // Download the blob's contents and save it to a file
                    // Get a reference to a blob named "sample-file"
                    BlobClient blobClient = containerClient.GetBlobClient(blob.Name);
                    BlobDownloadInfo download = await blobClient.DownloadAsync();

                    byte[] bytes;
                    using (MemoryStream stream = new MemoryStream())
                    {
                        await download.Content.CopyToAsync(stream);
                        bytes = stream.ToArray();

                    }

                    String txt = new String(Encoding.ASCII.GetString(bytes));

                    CommentBlobDTO blobDTO;
                    blobDTO.Name = blob.Name;
                    blobDTO.Contents = txt;
                    blobs.Add(blobDTO);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return blobs;

        }
    }

    public struct CommentBlobDTO
    {
        public string Name;
        public string Contents;

    }
}