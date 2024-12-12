namespace BlazorAzureADB2CApp1.Models
{
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using Azure.Storage.Sas;
    using Azure.Storage;
    using Microsoft.EntityFrameworkCore;
    using Azure.Identity;
    using Microsoft.AspNetCore.Components.Forms;

    public class BlobStorageService
    {
        private readonly IConfiguration _configuration;
        private readonly TestContext _dbContext;

        public BlobStorageService(IConfiguration configuration, TestContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public async Task<List<BlobFileInfo>> LoadFilesAsync(int parentId)
        {
            var accountName = _configuration["AzureStorageConfig:AccountName"];
            var containerName = _configuration["AzureStorageConfig:ContainerName"];
            var accountKey = _configuration["AzureStorageConfig:AccountKey"];

            string containerEndPoint = string.Format("https://{0}.blob.core.windows.net/{1}", accountName, containerName);

            var credential = new StorageSharedKeyCredential(accountName, accountKey);
            BlobContainerClient containerClient = new BlobContainerClient(new Uri(containerEndPoint), credential);

            // データベースから PhotoLocation を取得
            var routes = await _dbContext.Routs
                .Where(r => r.ParentId == parentId && r.PhotoLocation != null)
                .Select(r => r.PhotoLocation)
                .ToListAsync();

            // BLOB のリストを取得
            var files = new List<BlobFileInfo>();

            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                if (routes.Contains(blobItem.Name))
                {
                    var blobClient = containerClient.GetBlobClient(blobItem.Name);

                    // SAS トークンの生成
                    var sasBuilder = new BlobSasBuilder
                    {
                        BlobContainerName = containerName,
                        BlobName = blobItem.Name,
                        Resource = "b",
                        ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
                    };
                    sasBuilder.SetPermissions(BlobSasPermissions.Read);

                    var sasToken = sasBuilder.ToSasQueryParameters(credential);
                    var sasUri = new Uri($"{blobClient.Uri}?{sasToken}");

                    files.Add(new BlobFileInfo
                    {
                        Name = blobItem.Name,
                        Url = sasUri.ToString()
                    });
                }
            }

            return files;
        }

        public async Task UploadFilesAsync(int parentId, IList<IBrowserFile> files)
        {
            var accountName = _configuration["AzureStorageConfig:AccountName"];
            var containerName = _configuration["AzureStorageConfig:ContainerName"];
            var clientId = _configuration["AzureStorageConfig:ClientId"];

            string containerEndPoint = string.Format("https://{0}.blob.core.windows.net/{1}", accountName, containerName);

            BlobContainerClient containerClient = new BlobContainerClient(new Uri(containerEndPoint),
                                                                          new ManagedIdentityCredential(clientId));

            var routeModels = new List<Rout>();

            foreach (var file in files)
            {
                var newFileName = $"{parentId}_map_{DateTime.UtcNow:yyyyMMddHHmmss}{Path.GetExtension(file.Name)}";
                var blobClient = containerClient.GetBlobClient(newFileName);
                routeModels.Add(new Rout
                {
                    ParentId = parentId,
                    PhotoLocation = newFileName,
                    CreatedAt = DateTime.Now
                });

                using var stream = file.OpenReadStream();
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            // データベースにファイル情報を登録
            _dbContext.Routs.AddRange(routeModels);
            await _dbContext.SaveChangesAsync();
        }

        // BlobStorageService のメソッド
        public async Task DeleteFileAsync(BlobFileInfo file, int parentId)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file), "ファイル情報が null です。");
            }

            var accountName = _configuration["AzureStorageConfig:AccountName"];
            var containerName = _configuration["AzureStorageConfig:ContainerName"];
            var clientId = _configuration["AzureStorageConfig:ClientId"];

            string containerEndPoint = string.Format("https://{0}.blob.core.windows.net/{1}", accountName, containerName);

            BlobContainerClient containerClient = new BlobContainerClient(new Uri(containerEndPoint),
                                                                          new ManagedIdentityCredential(clientId));

            var blobClient = containerClient.GetBlobClient(file.Name);

            // BLOB ファイルを削除
            await blobClient.DeleteIfExistsAsync();

            // データベースからエントリを削除
            var route = await _dbContext.Routs.FirstOrDefaultAsync(r => r.ParentId == parentId && r.PhotoLocation == file.Name);
            if (route != null)
            {
                _dbContext.Routs.Remove(route);
                await _dbContext.SaveChangesAsync();
            }
        }

    }

    public class BlobFileInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }

}
