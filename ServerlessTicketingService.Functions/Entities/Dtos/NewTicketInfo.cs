using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessTicketingService.Functions.Entities.Dtos
{
    public class NewTicketInfo
    {
        public string Summary { get; set; }
        public string Description { get; set; }
        public string SenderEmail { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
