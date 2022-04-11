using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using ServerlessTicketingService.Functions.Entities.Models;
using ServerlessTicketingService.Functions.Orchestrators.Dtos;
using ServerlessTicketingService.Functions.Responses;

namespace ServerlessTicketingService.Functions.Orchestrators
{
    public class TicketUpdateOrchestrator
    {

        [FunctionName("TicketUpdateOrchestrator")]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger logger)
        {
            var ticket = context.GetInput<TicketUpdateNotificationDto>();

            var saveTicketResponse = await context.CallActivityAsync<bool>("SaveTicketToDatabaseActivity", ticket);

            if (saveTicketResponse)
                await context.CallActivityAsync("NotifyUpdateActivity", ticket);
        }
    }
}