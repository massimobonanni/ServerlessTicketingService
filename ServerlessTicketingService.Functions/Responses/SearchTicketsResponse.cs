using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ServerlessTicketingService.Functions.Responses
{
    public class SearchTicketsResponse
    {
        [OpenApiProperty(Description = "The value of the sender filter passed in the request")]
        [JsonProperty("senderFilter")]
        public string SenderFilter { get; set; }

        [OpenApiProperty(Description = "The value of the states filter passed in the request")]
        [JsonProperty("statesFilter")]
        public string StatesFilter { get; set; }

        [OpenApiProperty(Description = "The list of the tickets resulted in the search")]
        [JsonProperty("tickets")]
        public List<TicketDTO> Tickets { get; set; }
    }
}
