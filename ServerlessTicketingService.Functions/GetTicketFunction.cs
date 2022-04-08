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

namespace ServerlessTicketingService.Functions
{
    public class GetTicketFunction
    {
        private readonly ILogger _logger;

        public GetTicketFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetTicketFunction>();
        }

        [FunctionName("GetTicket")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "tickets/{ticketId}")] HttpRequest req,
            string ticketId)
        {
            _logger.LogInformation("GetTicket function");
            IActionResult responseData = null;

            try
            {
                var response = new TicketDTO() { Id = ticketId };

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
