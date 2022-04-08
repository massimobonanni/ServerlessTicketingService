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
    public class UpdateTicketFunction
    {
        private readonly ILogger _logger;

        public UpdateTicketFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<UpdateTicketFunction>();
        }

        [FunctionName("UpdateTicket")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "tickets/{ticketId}")] HttpRequest req,
            string ticketId)
        {
            _logger.LogInformation("UpdateTicket function");
            IActionResult responseData = null;

            try
            {
                string payloadContent = await new StreamReader(req.Body).ReadToEndAsync();
                var request = JsonConvert.DeserializeObject<UpdateTicketRequest>(payloadContent);

                if (request != null && request.IsValid())
                {

                    var response = new UpdateTicketResponse()
                    {
                        TicketId = ticketId
                    };

                    responseData = new OkObjectResult(response);
                }
                else
                {
                    responseData = new BadRequestResult();
                }
            }
            catch
            {
                responseData = new BadRequestResult();
            }

            return responseData;
        }
    }
}
