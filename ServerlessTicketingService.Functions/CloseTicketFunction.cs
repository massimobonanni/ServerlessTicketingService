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
    public class CloseTicketFunction
    {
        private readonly ILogger _logger;

        public CloseTicketFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CloseTicketFunction>();
        }

        [OpenApiOperation(operationId: "closeTicket",
             new[] { "Ticket Management" },
             Summary = "Close an existing ticket",
             Description = "Add a comment to an existing ticket and close it.",
             Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter("ticketId",
             Summary = "The id for the ticket to close",
             Description = "The id for the ticket to close with a comment",
             In = Microsoft.OpenApi.Models.ParameterLocation.Path,
             Required = true,
             Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json",
             bodyType: typeof(CloseTicketRequest),
             Description = "The new comment for the existing ticket to close.",
             Required = true)]
        [OpenApiResponseWithBody(HttpStatusCode.OK,
             "application/json",
             typeof(CloseTicketResponse),
             Summary = "Close an existing ticket response.",
             Description = "If the request is valid, the response contains the id for the closed ticket.")]
        [OpenApiResponseWithoutBody(HttpStatusCode.NotFound,
            Summary = "The ticket doesn't exist",
            Description = "The ticket id doesn't belong to an existing ticket.")]
        [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest,
             Summary = "The request is not valid",
             Description = "The contributor mail is not valid or the commenty is not present.")]
        
        [FunctionName("CloseTicket")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "tickets/{ticketId}/close")] HttpRequest req,
            string ticketId,
            [DurableClient] IDurableEntityClient client)
        {
            _logger.LogInformation("CloseTicket function");
            IActionResult responseData = null;

            try
            {
                string payloadContent = await new StreamReader(req.Body).ReadToEndAsync();
                var request = JsonConvert.DeserializeObject<CloseTicketRequest>(payloadContent);

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

                        await client.SignalEntityAsync<ITicket>(entityId, e => e.Close(ticketUpdate));

                        var response = new CloseTicketResponse()
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
