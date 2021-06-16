using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BlazorBlobStorage.Server.Core.Interfaces;
using BlazorBlobStorage.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorBlobStorage.Server.Infrastructure.Repositories {
    public class BlobStoragePicturesRepository : IPicturesRepository {
        private readonly BlobServiceClient blobServiceClient;
        private readonly ILogger<BlobStoragePicturesRepository> logger;

        public BlobStoragePicturesRepository(BlobServiceClient blobServiceClient, ILogger<BlobStoragePicturesRepository> logger) {
            this.blobServiceClient = blobServiceClient;
            this.logger = logger;
        }
        public async Task<IEnumerable<Picture>> GetPictures() {
            var containerClient = blobServiceClient.GetBlobContainerClient("demo");
            var results = new List<Picture>();
            await foreach (BlobItem blob in containerClient.GetBlobsAsync()) {
                BlobClient blobClient = containerClient.GetBlobClient(blob.Name);

                BlobProperties properties = await blobClient.GetPropertiesAsync();
                
                Picture picture = new Picture();
                picture.FileName = blob.Name;
                picture.ContentType = properties.ContentType;
                picture.FileContent = blobClient.OpenRead();
                results.Add(picture);
            }
            return results;
        }

        public async Task<UploadReply> UploadPicture(Picture picture) {
            //BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=simodemo01sa;AccountKey=H+f6fe3+oJLjvcIg403FlyK3ahLaNlqaGdXNon2l6IGepp877Ep1Q78Bhblb4kcx/d7OSjOb7wkIPC6IZu5n1A==;EndpointSuffix=core.windows.net");
            //BlobServiceClient blobServiceClient = new BlobServiceClient(new Uri("https://simodemo01sa.blob.core.windows.net/"), new VisualStudioCredential());


            //BlobServiceClient blobServiceClient = 
            //    new BlobServiceClient(new Uri("https://simodemo01sa.blob.core.windows.net/"), 
            //    new DefaultAzureCredential(new DefaultAzureCredentialOptions() { 
            //        AuthorityHost = Azure.Identity.AzureAuthorityHosts.AzurePublicCloud,
            //        ExcludeInteractiveBrowserCredential = false,
            //        ExcludeAzurePowerShellCredential = true,
            //        ExcludeAzureCliCredential = true,
            //        ExcludeEnvironmentCredential = true,
            //        ExcludeVisualStudioCodeCredential = true,
            //        ExcludeVisualStudioCredential = true
            //}));

            var containerClient = blobServiceClient.GetBlobContainerClient("demo");
            BlobClient blobClient = containerClient.GetBlobClient(picture.FileName);
            //BlobProperties properties = blobClient.GetProperties();

            BlobHttpHeaders headers = new BlobHttpHeaders {
                // Set the MIME ContentType every time the properties 
                // are updated or the field will be cleared
                ContentType = picture.ContentType,
                //ContentLanguage = "en-us",

                // Populate remaining headers with 
                // the pre-existing properties
                //CacheControl = properties.CacheControl,
                //ContentDisposition = properties.ContentDisposition,
                //ContentEncoding = properties.ContentEncoding,
                //ContentHash = properties.ContentHash
            };
            try {
                // Set the blob's properties.
                //await blobClient.SetHttpHeadersAsync(headers);

                //Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);
                logger.LogInformation("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

                // Open the file and upload its data
                await blobClient.UploadAsync(picture.FileContent, headers);
                return new UploadReply() { Message = $"Image uploaded at {blobClient.Uri}" };
            } catch (Exception ex) {
                logger.LogError(ex, "Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);
                return new UploadReply() { Message = $"{ex.GetType().Name}: {ex.Message}" };
            }
        }
    }
}
