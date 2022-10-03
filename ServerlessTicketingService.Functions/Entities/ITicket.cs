using ServerlessTicketingService.Functions.Entities.Dtos;
using ServerlessTicketingService.Functions.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessTicketingService.Functions.Entities
{
    public interface ITicket
    {
        void Initialize(NewTicketInfo ticket);
        void Update(UpdateTicketInfo ticketUpdate);
        void Close(UpdateTicketInfo ticketUpdate);
    }
}
