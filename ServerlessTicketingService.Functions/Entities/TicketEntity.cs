using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using ServerlessTicketingService.Functions.Entities.Dtos;
using ServerlessTicketingService.Functions.Entities.Models;
using ServerlessTicketingService.Functions.Orchestrators.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServerlessTicketingService.Functions.Entities
{
    public class TicketEntity : ITicket
    {
        private readonly ILogger _logger;
        public TicketEntity(ILogger logger)
        {
            _logger = logger;
        }

        [JsonPropertyName("ticket")]
        public TicketData Ticket { get; set; }

        public void Close(UpdateTicketInfo ticketUpdate)
        {
            if (ticketUpdate == null)
                return;

            var oldStatus = this.Ticket.Status;
            UpdateTicket(ticketUpdate, TicketStatus.Closed);
            NotifyUpdate(ticketUpdate, oldStatus);
        }

        public void Initialize(NewTicketInfo ticket)
        {
            if (ticket == null)
                return;

            if (this.Ticket == null)
                this.Ticket = new TicketData();

            if (this.Ticket.Status != TicketStatus.Unknown)
                return;

            this.Ticket.Summary = ticket.Summary;
            this.Ticket.SenderEmail = ticket.SenderEmail;
            this.Ticket.Description = ticket.Description;
            this.Ticket.CreationTimestamp = this.Ticket.LastUpdateTimestamp = ticket.Timestamp;
            this.Ticket.Status = TicketStatus.Open;
        }

        public void Update(UpdateTicketInfo ticketUpdate)
        {
            if (ticketUpdate == null)
                return;

            var oldStatus = this.Ticket.Status;
            UpdateTicket(ticketUpdate, TicketStatus.InProgress);
            NotifyUpdate(ticketUpdate, oldStatus);
        }

        private void UpdateTicket(UpdateTicketInfo ticketUpdate, TicketStatus newStatus)
        {
            if (this.Ticket == null || this.Ticket.Status == TicketStatus.Closed)
                return;

            if (this.Ticket.LastUpdateTimestamp > ticketUpdate.Timestamp)
                return;

            this.Ticket.Updates.Add(new TicketUpdateData()
            {
                Comment = ticketUpdate.Comment,
                ContributorEmail = ticketUpdate.ContributorEmail,
                Timestamp = ticketUpdate.Timestamp
            });

            this.Ticket.LastUpdateTimestamp = this.Ticket.Updates.Max(u => u.Timestamp);
            this.Ticket.Status = newStatus;
        }

        private void NotifyUpdate(UpdateTicketInfo ticketUpdate, TicketStatus oldStatus)
        {
            var orchestratyorDto = new TicketUpdateNotificationDto()
            {
                TicketId = Entity.Current.EntityKey,
                CreationTimestamp = this.Ticket.CreationTimestamp,
                LastUpdateTimestamp = this.Ticket.LastUpdateTimestamp,
                Description = this.Ticket.Description,
                LastUpdateComment = ticketUpdate.Comment,
                LastUpdateContributorEmail = ticketUpdate.ContributorEmail,
                OldStatus = oldStatus.ToString(),
                NewStatus = this.Ticket.Status.ToString(),
                SenderEmail = this.Ticket.SenderEmail,
                Summary = this.Ticket.Summary
            };

            Entity.Current.StartNewOrchestration("TicketUpdateOrchestrator", orchestratyorDto);
        }

        [FunctionName(nameof(TicketEntity))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx, ILogger logger)
                => ctx.DispatchAsync<TicketEntity>(logger);
    }
}
