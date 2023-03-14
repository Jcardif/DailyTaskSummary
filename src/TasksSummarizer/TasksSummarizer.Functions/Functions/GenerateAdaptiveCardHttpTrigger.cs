using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TaskSummarizer.Shared.Services;
using static TaskSummarizer.Shared.Helpers.OpenAiHelpers;


namespace TasksSummarizer.Functions.Functions
{
    public class GenerateAdaptiveCardHttpTrigger
    {
        private readonly ILogger _logger;

        public GenerateAdaptiveCardHttpTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GenerateAdaptiveCardHttpTrigger>();
        }

        [Function("GenerateAdaptiveCardHttpTrigger")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            // get tasksSummary from json body
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<dynamic>(requestBody);

            string? taskSummary = data?["taskSummary"];
            HttpResponseData? response;
            
            if (string.IsNullOrEmpty(taskSummary))
            {
                response = req.CreateResponse(HttpStatusCode.BadRequest);
                await response.WriteStringAsync("Please pass a taskSummary in the request body");
            }

            // Get settings from local.setting
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var apiKey = config.GetValue<string>("AzureOpenAI:APIKey");
            var deploymentId = config.GetValue<string>("AzureOpenAI:DeploymentId");
            var baseUrl = config.GetValue<string>("AzureOpenAI:BaseUrl");

            var filePath = Path.Combine(Environment.CurrentDirectory, "Prompts", "GenerateAdaptiveCard.txt");
            var baseSystemMessage = await File.ReadAllTextAsync(filePath);

            var chatService = new OpenAiChatService(apiKey, baseUrl, deploymentId);
            var prompt = GetAdaptiveCardPrompt(taskSummary, baseSystemMessage);
            var openAiResponse = await chatService.CreateCompletionAsync(prompt);

            response = req.CreateResponse(HttpStatusCode.OK);
            await HttpResponseDataExtensions.WriteStringAsync(response, openAiResponse?.Choices?.FirstOrDefault()?.Text ?? "Nothing to show");

            return response;
        }
    }
}
