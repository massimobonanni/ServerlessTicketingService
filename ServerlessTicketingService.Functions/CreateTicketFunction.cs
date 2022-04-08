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
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using System.Net;

namespace ServerlessTicketingService.Functions
{
    public class CreateTicketFunction
    {
        private readonly ILogger _logger;

        public CreateTicketFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CreateTicketFunction>();
        }

        [OpenApiOperation(operationId: "createTicket",
            new[] { "Ticket Management" },
            Summary = "Create a new ticket",
            Description = "Create a new support ticket.",
            Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json",
            bodyType: typeof(NewTicketRequest),
            Description = "Info about the ticket to create.",
            Required = true)]
        [OpenApiResponseWithBody(HttpStatusCode.OK,
            "application/json",
            typeof(NewTicketResponse),
            Summary = "New ticket response.",
            Description = "If the request is valid, the response contains the ticket id for the new ticket.")]
        [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest,
            Summary = "The request is not valid",
            Description = "The sender mail is not valid or the summary is not present.")]
        
        [FunctionName("CreateTicket")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "tickets")] HttpRequest req)
        {
            _logger.LogInformation("CreateTicket function");
            IActionResult responseData = null;

            try
            {
                string payloadContent = await new StreamReader(req.Body).ReadToEndAsync();
                var request = JsonConvert.DeserializeObject<NewTicketRequest>(payloadContent);

                if (request!=null && request.IsValid())
                {
                    var response = new NewTicketResponse()
                    {
                        TicketId = Guid.NewGuid().ToString()
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
