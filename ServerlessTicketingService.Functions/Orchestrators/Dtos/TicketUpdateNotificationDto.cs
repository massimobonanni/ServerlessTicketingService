using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessTicketingService.Functions.Orchestrators.Dtos
{
    public class TicketUpdateNotificationDto
    {
        public string TicketId { get; set; }
        public string SenderEmail { get; set; }
        public DateTimeOffset CreationTimestamp { get; set; }
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string LastUpdateComment { get; set; }
        public string LastUpdateContributorEmail { get; set; }
        public DateTimeOffset LastUpdateTimestamp { get; set; }
    }
}
