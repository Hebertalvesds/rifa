using System;
using System.Collections.Generic;
using cosmos_betinho.Entities;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace cosmos_betinho
{
    public static class AutomaticGenNumbers
    {
        [FunctionName("AutomaticGenNumbers")]
        public static void Run([CosmosDBTrigger(
            databaseName: "cosmos-betinho",
            containerName: "QuotasBilled",
            Connection = "CONNECTION_STRING",
            LeaseContainerName = "leases")]IReadOnlyList<QuotaEntity> input,
            ILogger log)
        {
            foreach (var inputItem in input)
            {

            }
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].Id);
            }
        }
    }
}
