using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace ServerlessTicketingService.Functions.Responses
{
    public class CloseTicketResponse
    {
        [OpenApiProperty(Description = "The Ticket Id of the closed ticket")]
        [JsonProperty("ticketId")]
        public string TicketId { get; set; }
    }
}
