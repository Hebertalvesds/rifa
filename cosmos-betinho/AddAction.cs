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
using System.Reflection.Metadata;
using cosmos_betinho.Entities;
using System.Data.SqlTypes;

namespace cosmos_betinho
{
    public static class AddAction
    {
        [FunctionName("AddAction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "add/action")] HttpRequest req,
            [CosmosDB(databaseName: "cosmos-betinho", containerName: "Actions"
            , Connection = "CONNECTION_STRING")]IAsyncCollector<dynamic> documentOut)
        {

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<Entities.ActionEntity>(requestBody);

                data.Id = Guid.NewGuid();

                await documentOut.AddAsync(new {
                    id = Guid.NewGuid(),
                    name = data.Name,
                    description = data.Description,
                    quotaValue = data.QuotaValue,
                    minimumQuotas = data.MinimumQuotas,
                    maxQuotaAllowed = data.MaxQuotaAllowed,
                    totalQuotas = data.TotalQuotas,
                    featured = data.Featured,
                    items = data.Items
                });

                return new OkObjectResult("Success!");
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            
        }

        
    }
}
