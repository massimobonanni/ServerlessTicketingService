using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using System.Text.Json.Serialization;

namespace ServerlessTicketingService.Functions.Responses
{
    public class NewTicketResponse
    {
        [OpenApiProperty(Description = "The Ticket Id of the new ticket")]
        [JsonPropertyName("ticketId")]
        public string TicketId { get; set; }
    }
}
