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
    public class GetTicketFunction
    {
        private readonly ILogger _logger;

        public GetTicketFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetTicketFunction>();
        }

        [OpenApiOperation(operationId: "getTicket",
            new[] { "Tickets Search" },
            Summary = "Retrieve the information about a ticket",
            Description = "Retrieve details info for an existing ticket.",
            Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter("ticketId",
            Summary = "The id for the ticket to retrieve",
            Description = "The id for the ticket to retrieve",
            In = Microsoft.OpenApi.Models.ParameterLocation.Path,
            Required = true,
            Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(HttpStatusCode.OK,
            "application/json",
            typeof(GetTicketResponse),
            Summary = "The ticket details",
            Description = "If the ticket exists, the response contains the detail of the ticket.")]
        [OpenApiResponseWithoutBody(HttpStatusCode.NotFound,
            Summary = "The ticket id doesn't exist",
            Description = "The ticket with the id used in the request doesn't exist.")]

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
