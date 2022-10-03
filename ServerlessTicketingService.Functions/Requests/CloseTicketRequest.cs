using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;

namespace ServerlessTicketingService.Functions.Requests
{
    public class CloseTicketRequest
    {
        [OpenApiProperty(Description = "The mail of the user that close the ticket")]
        [JsonProperty("contributorEmail")]
        public string ContributorEmail { get; set; }

        [OpenApiProperty(Description = "The date and time when the ticket was closed")]
        [JsonProperty("timestamp")] 
        public DateTimeOffset Timestamp { get; set; }

        [OpenApiProperty(Description = "The comment to add to the closing ticket")]
        [JsonProperty("comment")]
        public string Comment { get; set; }

        internal bool IsValid()
        {
            bool retVal = true;
            retVal &= !string.IsNullOrWhiteSpace(Comment);
            retVal &= !string.IsNullOrWhiteSpace(ContributorEmail);

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(ContributorEmail);
            retVal &= match.Success;

            return retVal;
        }
    }
}
