using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Messaging;
using Azure.Messaging.EventGrid;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServerlessTicketingService.Functions.Entities.Models;
using ServerlessTicketingService.Functions.Orchestrators.Dtos;
using ServerlessTicketingService.Functions.Responses;

namespace ServerlessTicketingService.Functions.Activities
{
    public class NotifyUpdateActivity
    {
        [FunctionName("NotifyUpdateActivity")]
        public async Task<bool> Run([ActivityTrigger] TicketUpdateNotificationDto ticket,
            [EventGrid(TopicEndpointUri = "TopicEndpoint", TopicKeySetting = "TopicKey")] IAsyncCollector<EventGridEvent> eventCollector,
            ILogger logger)
        {
            var @event = new EventGridEvent(
                subject: $"tickets/{ticket.TicketId}",
                eventType: "ticketUpdate",
                dataVersion: "1.0",
                data: ticket);

            await eventCollector.AddAsync(@event);

            return true;
        }

    }
}