using System;
using System.Text.RegularExpressions;

namespace ServerlessTicketingService.Functions.Requests
{
    public class NewTicketRequest
    {

        public string Summary { get; set; }

        public string Description { get; set; }

        public string SenderEmail { get; set; }

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
