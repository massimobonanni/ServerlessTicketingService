using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ServerlessTicketingService.Functions.Responses
{
    public class TicketDTO
    {
        [OpenApiProperty(Description = "The ticket id")]
        [JsonProperty("id")] 
        public string Id { get; set; }

        [OpenApiProperty(Description = "The email of the ticket sender")]
        [JsonProperty("senderEmail")]
        public string SenderEmail { get; set; }

        [OpenApiProperty(Description = "The creation timestamp")]
        [JsonProperty("creationTimestamp")] 
        public DateTimeOffset CreationTimestamp { get; set; }

        [OpenApiProperty(Description = "The last update timestamp")]
        [JsonProperty("lastUpdateTimestamp")] 
        public DateTimeOffset LastUpdateTimestamp { get; set; }

        [OpenApiProperty(Description = "The ticket status")]
        [JsonProperty("status")] 
        public string Status { get; set; }

        [OpenApiProperty(Description = "The list of updates of the tickets")]
        [JsonProperty("updates")] 
        public List<TicketUpdateDto> Updates { get; set; } = new List<TicketUpdateDto>();

        [OpenApiProperty(Description = "The ticket summary")]
        [JsonProperty("summary")] 
        public string Summary { get; set; }

        [OpenApiProperty(Description = "The ticket description")]
        [JsonProperty("description")] 
        public string Description { get; set; }
    }
}