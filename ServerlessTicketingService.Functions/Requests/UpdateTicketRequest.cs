using System;
using System.Text.RegularExpressions;

namespace ServerlessTicketingService.Functions.Requests
{
    public class UpdateTicketRequest
    {
        public string ContributorEmail { get; set; }

        public DateTimeOffset Timestamp { get; set; }

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
