using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Text;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

namespace cosmos_betinho
{
    public static class GetDocument
    {
        [FunctionName("GetAllVendors")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "vendors")] HttpRequest request,
            ILogger log)
        {
            //string token = request.Query.Count > 0 ? (request.Query.Keys.Contains("continuation_token") ? request.Query["continuation_token"] : string.Empty) : string.Empty;
            var offset = request.Query.Count > 0 ? (request.Query.Keys.Contains("offset") ? request.Query["offset"] : "0") : "0";
            var limit = request.Query.Count > 0 ? (request.Query.Keys.Contains("limit") ? request.Query["limit"] : "100") : "100";

            QueryDefinition query = new QueryDefinition($"SELECT * FROM c OFFSET {offset} LIMIT {limit}");

            var cosmosClient = new CosmosClient(Environment.GetEnvironmentVariable("connectionStringLocal") );
            var database = cosmosClient.GetDatabase(Environment.GetEnvironmentVariable("cosmosDB"));
            var container = database.GetContainer("vendors");

            var iterator = container.GetItemQueryIterator<Vendor>(query, requestOptions: new QueryRequestOptions { MaxItemCount = int.Parse(limit) });

            try
            {
                if (iterator.HasMoreResults)
                {
                    FeedResponse<Vendor> items = await iterator.ReadNextAsync();
                    return new OkObjectResult(items);
                }
                else
                    return new OkObjectResult("End of the list.");
                
            }
            catch(Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
        }

        public class Vendor
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Telefone { get; set; }
        };
    }
}
