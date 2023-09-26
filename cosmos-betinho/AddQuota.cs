using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace cosmos_betinho
{
    public static class AddQuota
    {
        [FunctionName("AddQuota")]
        public static async Task<IActionResult> Run(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "add/quota")] HttpRequest req,
            [CosmosDB(databaseName: "cosmos-betinho", containerName: "QuotasBilled"
            , Connection = "CONNECTION_STRING")]IAsyncCollector<dynamic> documentOut)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<Entities.QuotaEntity>(requestBody);

                data.Id = Guid.NewGuid();

                await documentOut.AddAsync(new
                {
                    id = data?.Id ?? Guid.NewGuid(),
                    name = data.Name,
                    quotasAquired = data.QuotasAquired,
                    email = data.Email,
                    totalQuotasAquired = data.TotalQuotasAquired,
                    phone = data.Phone,
                    vendor = data.Vendor,
                    totalBilled = data.TotalBilled,
                    billedAt = data.BilledAt,
                    atachLink = data.AtachLink,
                    atachedAt = data.AtachedAt,
                    thirdEmail = data.ThirdEmail
                });

                return new OkObjectResult("Success");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
