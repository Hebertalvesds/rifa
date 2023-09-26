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
using System.Collections.Generic;

namespace cosmos_betinho
{
    public static class GetUnpaidQuotas
    {
        [FunctionName("GetAllQuotas")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "get/quotas/unpaid/{offset:int}/{limit:int}")] HttpRequest req,
            int offset, int limit, ILogger log)
        {
            QueryDefinition query = new QueryDefinition($"SELECT * FROM c WHERE c.atachedAt = null OFFSET {offset} LIMIT {limit}");
            var cosmosClient = new CosmosClient(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
            var database = cosmosClient.GetDatabase(Environment.GetEnvironmentVariable("cosmosDB"));
            var container = database.GetContainer("QuotasBilled");

            var iterator = container.GetItemQueryIterator<Entities.QuotaEntity>(query, requestOptions: new QueryRequestOptions { MaxItemCount = limit });

            try
            {
                FeedResponse<Entities.QuotaEntity> items = await iterator.ReadNextAsync();
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
