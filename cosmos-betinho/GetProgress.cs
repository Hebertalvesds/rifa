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
using System.Linq;
using cosmos_betinho.Entities;

namespace cosmos_betinho
{
    public static class GetProgress
    {
        [FunctionName("GetProgress")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "get/progress/{actionId}")] HttpRequest req,
            string actionId, ILogger log)
        {
            QueryDefinition query = new QueryDefinition($"SELECT * FROM c WHERE c.id = '{actionId}'");
            var cosmosClient = new CosmosClient(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
            var database = cosmosClient.GetDatabase(Environment.GetEnvironmentVariable("cosmosDB"));
            var container = database.GetContainer("Actions");

            var iterator = container.GetItemQueryIterator<Entities.ActionEntity>(query, requestOptions: new QueryRequestOptions { MaxItemCount = 1 });

            try
            {
                decimal progress = 0;

                if (iterator.HasMoreResults)
                {
                    FeedResponse<Entities.ActionEntity> items = await iterator.ReadNextAsync();
                    var action = items.FirstOrDefault();

                    query = new QueryDefinition($"SELECT VALUE SUM(c.totalQuotasAquired) FROM c WHERE c.actionId = '{actionId}'");
                    container = database.GetContainer("QuotasBilled");
                    var qIterator = container.GetItemQueryIterator<decimal>(query);
                    var quotas = await qIterator.ReadNextAsync();
                    progress = (quotas.FirstOrDefault() * 100) / action.TotalQuotas;

                }
                
                return new OkObjectResult(progress);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
