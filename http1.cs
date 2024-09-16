using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace privmanfunc56fa
{
    public class http1
    {
        private readonly ILogger<http1> _logger;

        public http1(ILogger<http1> logger)
        {
            _logger = logger;
        }

        [Function("http1")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            string blobConnString = Environment.GetEnvironmentVariable("storConnString") ?? "FAIL";
            string blobContainerName = "blobcontainer";
            string blobName = "blobname";

            _logger.LogInformation("C# HTTP trigger function processed a request.");

            if (string.IsNullOrEmpty(blobConnString))
            {
                return new BadRequestObjectResult("Please provide a connection string for the storage account.");
            }

            var blobServiceClient = new BlobServiceClient(new Uri(blobConnString), new DefaultAzureCredential());

            // Get a reference to the Blob Container
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);

            // Get a reference to the Blob
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            // Download the Blob content to a stream
            try
            {
                Console.WriteLine($"Reading blob '{blobName}' from container '{blobContainerName}'...");
                BlobDownloadInfo download = await blobClient.DownloadAsync();

                using (var reader = new StreamReader(download.Content))
                {
                    string blobContent = await reader.ReadToEndAsync();
                    Console.WriteLine("Blob content:");
                    Console.WriteLine(blobContent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading blob: {ex.Message}");
            }

            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
