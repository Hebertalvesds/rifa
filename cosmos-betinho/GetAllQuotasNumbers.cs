using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;

namespace cosmos_betinho
{
    public static class GetAllQuotasNumbers
    {
        [FunctionName("GetAllQuotasNumbers")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "get/quotas/all")] HttpRequest req,
            ILogger log)
        {
            try
            {
                QueryDefinition query = new QueryDefinition($"SELECT c.quotasAquired FROM c");
                var cosmosClient = new CosmosClient(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
                var database = cosmosClient.GetDatabase(Environment.GetEnvironmentVariable("cosmosDB"));
                var container = database.GetContainer("QuotasBilled");

                var iterator = container.GetItemQueryIterator<Entities.QuotasAquiredEntity>(query);

                FeedResponse<Entities.QuotasAquiredEntity> items = await iterator.ReadNextAsync();
                return new OkObjectResult(items);

            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
