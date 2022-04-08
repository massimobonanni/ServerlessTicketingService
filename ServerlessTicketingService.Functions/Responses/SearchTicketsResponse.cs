using System.Collections.Generic;

namespace ServerlessTicketingService.Functions.Responses
{
    public class SearchTicketsResponse
    {
        public string SenderFilter { get; set; }
        public string StatesFilter { get; set; }
        public List<TicketDTO> Tickets { get; set; }
    }
}
