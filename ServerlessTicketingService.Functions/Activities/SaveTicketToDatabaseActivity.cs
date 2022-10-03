using System;
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

namespace ServerlessTicketingService.Functions.Activities
{
    public class SaveTicketToDatabaseActivity
    {
        public class TicketStorageEntity
        {
            public string PartitionKey { get; set; }
            public string RowKey { get; set; }
            public string Id { get; set; }
            public string SenderEmail { get; set; }
            public DateTimeOffset CreationTimestamp { get; set; }
            public string OldStatus { get; set; }
            public string NewStatus { get; set; }
            public string Summary { get; set; }
            public string Description { get; set; }
            public string LastUpdateComment { get; set; }
            public string LastUpdateContributorEmail { get; set; }
            public DateTimeOffset LastUpdateTimestamp { get; set; }
        }
                
        [FunctionName("SaveTicketToDatabaseActivity")]
        public async Task<bool> Run([ActivityTrigger] TicketUpdateNotificationDto ticket,
            [Table("Tickets", Connection = "TicketStorageConnection")] IAsyncCollector<TicketStorageEntity> table,
            ILogger logger)
        {
            var ticketEntity = new TicketStorageEntity()
            {
                PartitionKey = ticket.TicketId,
                RowKey = Guid.NewGuid().ToString(),
                Id = ticket.TicketId,
                SenderEmail = ticket.SenderEmail,
                CreationTimestamp = ticket.CreationTimestamp,
                OldStatus = ticket.OldStatus,
                NewStatus = ticket.NewStatus,
                Summary = ticket.Summary,
                Description = ticket.Description,
                LastUpdateComment = ticket.LastUpdateComment,
                LastUpdateContributorEmail = ticket.LastUpdateContributorEmail,
                LastUpdateTimestamp = ticket.LastUpdateTimestamp
            };

            try
            {
                await table.AddAsync(ticketEntity);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}