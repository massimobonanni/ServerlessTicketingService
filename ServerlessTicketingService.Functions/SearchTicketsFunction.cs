using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServerlessTicketingService.Functions.Requests;
using ServerlessTicketingService.Functions.Responses;
using System.Collections.Generic;

namespace ServerlessTicketingService.Functions
{
    public class SearchTicketsFunction
    {
        private readonly ILogger _logger;

        public SearchTicketsFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SearchTicketsFunction>();
        }

        [FunctionName("SearchTickets")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "tickets")] HttpRequest req)
        {
            _logger.LogInformation("SearchTickets function");
            IActionResult responseData = null;

            string senderFilter = req.Query["sender"];
            string statesFilter = req.Query["states"];

            try
            {
                var response = new SearchTicketsResponse()
                {
                    SenderFilter = senderFilter,
                    StatesFilter = statesFilter
                };
                response.Tickets = new List<TicketDTO>() {
                    new TicketDTO() { Id = Guid.NewGuid().ToString() },
                    new TicketDTO() { Id = Guid.NewGuid().ToString() },
                };

                responseData = new OkObjectResult(response);
            }
            catch
            {
                responseData = new BadRequestResult();
            }

            return responseData;
        }
    }
}
