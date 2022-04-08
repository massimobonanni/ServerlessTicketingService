using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;
using System;

namespace ServerlessTicketingService.Functions.Responses
{
    public class TicketUpdateDto
    {
        [OpenApiProperty(Description = "The ticket update comment")]
        [JsonProperty("comment")] 
        public string Comment { get; set; }

        [OpenApiProperty(Description = "The email of the contributor of the update")]
        [JsonProperty("contributorEmail")] 
        public string ContributorEmail { get; set; }

        [OpenApiProperty(Description = "The update timestamp")]
        [JsonProperty("timestamp")] 
        public DateTimeOffset Timestamp { get; set; }
    }
}