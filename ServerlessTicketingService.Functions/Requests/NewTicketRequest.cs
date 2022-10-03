using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;

namespace ServerlessTicketingService.Functions.Requests
{
    public class NewTicketRequest
    {
        [OpenApiProperty(Description = "The summary for the new ticket")]
        [JsonProperty("summary")]
        public string Summary { get; set; }

        [OpenApiProperty(Description = "The description for the new ticket")]
        [JsonProperty("description")]
        public string Description { get; set; }

        [OpenApiProperty(Description = "The email of the user who created the ticket")]
        [JsonProperty("senderEmail")]
        public string SenderEmail { get; set; }

        [OpenApiProperty(Description = "The date and time when the ticket was created")]
        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        internal bool IsValid()
        {
            bool retVal = true;
            retVal &= !string.IsNullOrWhiteSpace(Summary);
            retVal &= !string.IsNullOrWhiteSpace(SenderEmail);

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(SenderEmail);
            retVal &= match.Success;
            
            return retVal;
        }
    }
}
