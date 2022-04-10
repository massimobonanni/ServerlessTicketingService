using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessTicketingService.Functions.OpenApi
{
    public class OpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions
    {
        public override OpenApiInfo Info { get; set; } = new OpenApiInfo()
        {
            Version = "1.0.0",
            Title = "Serverless Ticketing System",
            Description = "The ultimate ticketing management service written with serverless technologies",
            TermsOfService = new Uri("https://github.com/massimobonanni/ServerlessTicketingService"),
            Contact = new OpenApiContact()
            {
                Name = "Serverless Ticketing Service Support",
                Email = "support@serverlessticketingservice.com",
                Url = new Uri("https://github.com/massimobonanni/ServerlessTicketingService"),
            },
            License = new OpenApiLicense()
            {
                Name = "MIT",
                Url = new Uri("http://opensource.org/licenses/MIT"),
            }
        };

    }
}
