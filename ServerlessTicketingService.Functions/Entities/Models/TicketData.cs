using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessTicketingService.Functions.Entities.Models
{
    public class TicketData
    {
        public string SenderEmail { get; set; }
        public DateTimeOffset CreationTimestamp { get; set; }
        public DateTimeOffset LastUpdateTimestamp { get; set; }
        public TicketStatus Status { get; set; } = TicketStatus.Unknown;
        public List<TicketUpdateData> Updates { get; set; } = new List<TicketUpdateData>();
        public string Summary { get; set; }
        public string Description { get; set; }
    }
}
