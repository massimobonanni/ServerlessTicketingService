using System;

namespace ServerlessTicketingService.Functions.Entities.Models
{
    public class TicketUpdateData
    {
        public string Comment { get; set; }
        public string ContributorEmail { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}