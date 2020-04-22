using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureImageServiceFunctions
{
    public static class ImageUploadHandler
    {
        [FunctionName("ImageUploadHandler")]
        public static async Task Run(
            [BlobTrigger("images/{name}", Connection = "azurestorageaccount")]
        CloudBlockBlob blob, string name, ILogger log,
           [SignalR(HubName = "imagehub")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            var sas = GetSas(blob);

            var url = blob.Uri + sas;

            await signalRMessages.AddAsync(
              new SignalRMessage
              {
                  Target = "RecievedMessage",
                  Arguments = new[] { blob.Name, url }
              });
        }

        private static string GetSas(CloudBlockBlob blob)
        {
            var sasPolicy = new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-15),
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15)
            };

            var sas = blob.GetSharedAccessSignature(sasPolicy);

            return sas;
        }
    }
}
