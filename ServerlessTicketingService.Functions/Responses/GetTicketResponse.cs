using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace ServerlessTicketingService.Functions.Responses
{
    public class GetTicketResponse
    {
        [OpenApiProperty(Description = "The requested ticket")]
        [JsonProperty("ticket")]
        public TicketDTO Ticket { get; set; }
    }
}
