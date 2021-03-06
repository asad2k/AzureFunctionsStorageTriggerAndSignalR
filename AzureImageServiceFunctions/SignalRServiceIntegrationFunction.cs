using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace AzureImageServiceFunctions
{
    public static class SignalRServiceIntegrationFunction
    {
        [FunctionName("negotiate")]
        public static SignalRConnectionInfo GetSignalRInfo(
       [HttpTrigger(AuthorizationLevel.Anonymous)]HttpRequest req,
       [SignalRConnectionInfo(HubName = "imagehub", ConnectionStringSetting = "AzureSignalRConnectionString")]SignalRConnectionInfo connectionInfo)
        {

            return connectionInfo;
        }
    }
}
