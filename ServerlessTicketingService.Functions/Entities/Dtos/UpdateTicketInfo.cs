using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessTicketingService.Functions.Entities.Dtos
{
    public class UpdateTicketInfo
    {
        public string Comment { get; set; }
        public string ContributorEmail { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
