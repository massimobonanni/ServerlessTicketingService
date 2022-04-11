using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using ServerlessTicketingService.Functions.Entities.Dtos;
using ServerlessTicketingService.Functions.Entities.Models;
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

            UpdateTicket(ticketUpdate,TicketStatus.Closed);
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
            
            UpdateTicket(ticketUpdate, TicketStatus.InProgress);
        }


        private bool UpdateTicket(UpdateTicketInfo ticketUpdate, TicketStatus newStatus)
        {
            if (this.Ticket == null || this.Ticket.Status == TicketStatus.Closed)
                return false;

            if (this.Ticket.LastUpdateTimestamp > ticketUpdate.Timestamp)
                return false;

            this.Ticket.Updates.Add(new TicketUpdateData()
            {
                Comment = ticketUpdate.Comment,
                ContributorEmail = ticketUpdate.ContributorEmail,
                Timestamp = ticketUpdate.Timestamp
            });

            this.Ticket.LastUpdateTimestamp = this.Ticket.Updates.Max(u => u.Timestamp);
            this.Ticket.Status = newStatus;
            return true;
        }

        [FunctionName(nameof(TicketEntity))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx, ILogger logger)
                => ctx.DispatchAsync<TicketEntity>(logger);
    }
}
