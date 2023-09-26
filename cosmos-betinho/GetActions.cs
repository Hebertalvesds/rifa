using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Core;
using Microsoft.Azure.Cosmos;
using static cosmos_betinho.GetDocument;
using System.Collections.Generic;
using System.Configuration;

namespace cosmos_betinho
{
    public static class GetActions
    {
        [FunctionName("GetActions")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "get/actions/{offset:int}/{limit:int}")] HttpRequest req,
            int offset, int limit, ILogger log)
        {
            QueryDefinition query = new QueryDefinition($"SELECT * FROM c OFFSET {offset} LIMIT {limit}");
            var cosmosClient = new CosmosClient(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
            var database = cosmosClient.GetDatabase(Environment.GetEnvironmentVariable("cosmosDB"));
            var container = database.GetContainer("Actions");

            var iterator = container.GetItemQueryIterator<dynamic>(query, requestOptions: new QueryRequestOptions { MaxItemCount = limit });

            try
            {
                FeedResponse<dynamic> items = await iterator.ReadNextAsync();
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
