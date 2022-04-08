using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace ServerlessTicketingService.Functions.Responses
{
    public class UpdateTicketResponse
    {
        [OpenApiProperty(Description = "The Ticket Id of the updated ticket")]
        [JsonProperty("ticketId")] 
        public string TicketId { get; set; }
    }
}
