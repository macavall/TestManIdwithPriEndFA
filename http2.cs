using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace privmanfunc56fa
{
    public class http2
    {
        private readonly ILogger<http2> _logger;

        public http2(ILogger<http2> logger)
        {
            _logger = logger;
        }

        [Function("http2")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Process ID:" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString());
        }
    }
}
