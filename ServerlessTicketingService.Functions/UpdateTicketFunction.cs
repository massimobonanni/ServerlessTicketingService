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
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using ServerlessTicketingService.Functions.Entities;
using Newtonsoft.Json.Linq;
using ServerlessTicketingService.Functions.Entities.Dtos;

namespace ServerlessTicketingService.Functions
{
    public class UpdateTicketFunction
    {
        private readonly ILogger _logger;

        public UpdateTicketFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<UpdateTicketFunction>();
        }

        [OpenApiOperation(operationId: "updateTicket",
            new[] { "Ticket Management" },
            Summary = "Update an existing ticket",
            Description = "Add a comment to an existing ticket.",
            Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter("ticketId",
            Summary = "The id for the ticket to update",
            Description = "The id for the ticket to update with a comment",
            In = Microsoft.OpenApi.Models.ParameterLocation.Path,
            Required = true,
            Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json",
            bodyType: typeof(UpdateTicketRequest),
            Description = "The new comment for the existing ticket.",
            Required = true)]
        [OpenApiResponseWithBody(HttpStatusCode.OK,
            "application/json",
            typeof(UpdateTicketResponse),
            Summary = "Update an existing ticket response.",
            Description = "If the request is valid, the response contains the id for the updated ticket.")]
        [OpenApiResponseWithoutBody(HttpStatusCode.NotFound,
            Summary = "The ticket doesn't exist",
            Description = "The ticket id doesn't belong to an existing ticket.")]
        [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest,
            Summary = "The request is not valid",
            Description = "The contributor mail is not valid or the commenty is not present.")]

        [FunctionName("UpdateTicket")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "tickets/{ticketId}")] HttpRequest req,
            string ticketId,
            [DurableClient] IDurableEntityClient client)
        {
            _logger.LogInformation("UpdateTicket function");
            IActionResult responseData = null;

            try
            {
                string payloadContent = await new StreamReader(req.Body).ReadToEndAsync();
                var request = JsonConvert.DeserializeObject<UpdateTicketRequest>(payloadContent);

                if (request != null && request.IsValid())
                {
                    var entityId = new EntityId(nameof(TicketEntity), ticketId);

                    EntityStateResponse<JObject> entity = await client.ReadEntityStateAsync<JObject>(entityId);
                    if (entity.EntityExists)
                    {
                        var ticketUpdate = new UpdateTicketInfo()
                        {
                            Comment = request.Comment,
                            ContributorEmail = request.ContributorEmail,
                            Timestamp = request.Timestamp
                        };

                        await client.SignalEntityAsync<ITicket>(entityId, e => e.Update(ticketUpdate));

                        var response = new UpdateTicketResponse()
                        {
                            TicketId = ticketId
                        };

                        responseData = new OkObjectResult(response);
                    }
                    else
                    {
                        responseData = new NotFoundResult();
                    }
                }
                else
                {

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
