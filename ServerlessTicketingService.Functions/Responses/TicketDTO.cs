using System;
using System.Collections.Generic;

namespace ServerlessTicketingService.Functions.Responses
{
    public class TicketDTO
    {
        public string Id { get; set; }
        public string SenderEmail { get; set; }
        public DateTimeOffset CreationTimestamp { get; set; }
        public DateTimeOffset LastUpdateTimestamp { get; set; }
        public string Status { get; set; }
        public List<TicketUpdateDto> Updates { get; set; } = new List<TicketUpdateDto>();
        public string Summary { get; set; }
        public string Description { get; set; }
    }
}