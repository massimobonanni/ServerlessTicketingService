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
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using System.Net;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using ServerlessTicketingService.Functions.Entities;
using Newtonsoft.Json.Linq;
using ServerlessTicketingService.Functions.Entities.Models;
using System.Linq;

namespace ServerlessTicketingService.Functions
{
    public class SearchTicketsFunction
    {
        private readonly ILogger _logger;

        public SearchTicketsFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SearchTicketsFunction>();
        }

        [OpenApiOperation(operationId: "searchTicket",
            new[] { "Tickets Search" },
            Summary = "Search the tickets based on filters",
            Description = "Retrieve the list of the tickets that verified the filters passed in the query string.",
            Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter("sender",
            Summary = "The mail (or part of the mail) of the ticker sender",
            Description = "The API returns all the tickets that contains the filter in the sender mail",
            In = Microsoft.OpenApi.Models.ParameterLocation.Query,
            Required = false,
            Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter("states",
            Summary = "The states of the ticker",
            Description = "The API returns all the tickets that are in one of the state of the filter. For example, 'open|closed' returns all the tickets closed or open.",
            In = Microsoft.OpenApi.Models.ParameterLocation.Query,
            Required = false,
            Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(HttpStatusCode.OK,
            "application/json",
            typeof(SearchTicketsResponse),
            Summary = "The list of the tickets that verify the filters passed in the request",
            Description = "The response contains the list of the tickets searched by the filters you pass in the quary string and the filters themself.")]


        [FunctionName("SearchTickets")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "tickets")] HttpRequest req,
            [DurableClient] IDurableEntityClient client)
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

                EntityQuery queryDefinition = new EntityQuery()
                {
                    PageSize = 100,
                    FetchState = true,
                    EntityName = nameof(TicketEntity)
                };

                var tickets = new List<TicketDTO>();
                do
                {
                    EntityQueryResult queryResult = await client.ListEntitiesAsync(queryDefinition, default);

                    foreach (var item in queryResult.Entities)
                    {
                        var entityState = (JObject)item.State;
                        var ticketProperty = (JObject)entityState.Property("Ticket").Value;
                        var ticket = ticketProperty.ToObject<TicketDTO>();
                        var ticketStatus = (TicketStatus)(int)ticketProperty.Property("Status").Value;
                        ticket.Status = ticketStatus.ToString();
                        ticket.Id = item.EntityId.EntityKey;
                        tickets.Add(ticket);
                    }

                    queryDefinition.ContinuationToken = queryResult.ContinuationToken;
                } while (queryDefinition.ContinuationToken != null);

                var filteredTickets = tickets.AsQueryable();
                if (!string.IsNullOrEmpty(senderFilter))
                {
                    filteredTickets = filteredTickets.Where(t => t.SenderEmail.Contains(senderFilter));
                }
                if (!string.IsNullOrEmpty(statesFilter))
                {
                    var states = statesFilter.ToLower().Split('|');
                    filteredTickets = filteredTickets.Where(t => states.Contains(t.Status.ToLower()));
                }

                response.Tickets = filteredTickets.ToList();

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
