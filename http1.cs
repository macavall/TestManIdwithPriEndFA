using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace privmanfunc56fa
{
    public class http1
    {
        private readonly ILogger<http1> _logger;
        private DefaultAzureCredential _credential;

        public http1(ILogger<http1> logger)
        {
            _logger = logger;
        }

        [Function("http1")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            string blobConnString = Environment.GetEnvironmentVariable("storConnString") ?? "FAIL";
            string blobContainerName = Environment.GetEnvironmentVariable("blobContainer") ?? "blobcontainer";
            string blobName = Environment.GetEnvironmentVariable("blobName") ?? "blobname.txt";
            string storEndpoint = Environment.GetEnvironmentVariable("storEndpoint") ?? "NoStorageEndpoint";
            string resultStr = String.Empty;

            string blobContent = String.Empty;

#if LOCAL
            Console.WriteLine("DEBUGDEBUGDEBUGDEBUGDEBUGDEBUGDEBUGDEBUGDEBUGDEBUGDEBUGDEBUGDEBUGDEBUGDEBUGDEBUGDEBUGDEBUGDEBUGDEBUGDEBUGDEBUGDEBUGDEBUGDEBUG");

            var blobClient2 = new BlobServiceClient(new Uri(Environment.GetEnvironmentVariable("storConnString")), new ManagedIdentityCredential());

            #endif



            _logger.LogInformation("C# HTTP trigger function processed a request.");

            if (string.IsNullOrEmpty(blobConnString))
            {
                return new OkObjectResult("Please provide a connection string for the storage account.");
            }

            try
            {
                var blobServiceClient = new BlobServiceClient(new Uri(storEndpoint), new DefaultAzureCredential());

                resultStr += "Create Blob Service Client Success\n";

                // Get a reference to the Blob Container
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);

                resultStr += "Container Client Created!!!\n";

                // Get a reference to the Blob
                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                resultStr += "GetBlobClient created!!!\n";

                // Download the Blob content to a stream
                try
                {
                    Console.WriteLine($"Reading blob '{blobName}' from container '{blobContainerName}'...");

                    BlobDownloadInfo download = await blobClient.DownloadAsync();

                    resultStr += "Client created!!!\n";

                    using (var reader = new StreamReader(download.Content))
                    {
                        resultStr += "About to Read!!!\n";

                        blobContent = await reader.ReadToEndAsync();

                        resultStr += "Read Complete!!!\n";

                        Console.WriteLine("Blob content:");
                        Console.WriteLine(blobContent);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading blob: {ex.Message}");
                    Console.WriteLine($"Error reading blob: {ex.StackTrace}");

                    return new OkObjectResult(resultStr);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Creating Blob Client Exception: {ex.Message}");
                Console.WriteLine($"Creating Blob Client Exception: {ex.StackTrace}");

                return new OkObjectResult(resultStr);
            }

            return new OkObjectResult(resultStr + "\n\n\n" + blobContent);
        }
    }
}
