using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TasksSummarizer.Functions.Functions
{
    public class GetCollaboratorsHttpTrigger
    {
        private readonly ILogger _logger;

        public GetCollaboratorsHttpTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetCollaboratorsHttpTrigger>();
        }

        [Function("GetCollaboratorsHttpTrigger")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<dynamic>(requestBody);

            string? collaborators = data?["collaborators"];
            HttpResponseData? response;

            if (string.IsNullOrEmpty(collaborators))
            {
                var error = new { collaborators = "" };

                response = req.CreateResponse(HttpStatusCode.BadRequest);
                await response.WriteAsJsonAsync(error);

                return response;
            }

            string pattern = @"(?<=@)\w+\s\w+";
            var matches = Regex.Matches(collaborators, pattern);

            var colLaboratorsValues = "";

            foreach (Match match in matches)
            {
                colLaboratorsValues += $"{match.Value};";
            }
            colLaboratorsValues = colLaboratorsValues.TrimEnd(';');

            var result = new { collaborators = colLaboratorsValues };
            response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }
    }
}
