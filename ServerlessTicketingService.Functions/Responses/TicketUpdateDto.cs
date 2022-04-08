using System;

namespace ServerlessTicketingService.Functions.Responses
{
    public class TicketUpdateDto
    {
        public string Comment { get; set; }
        public string ContributorEmail { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}