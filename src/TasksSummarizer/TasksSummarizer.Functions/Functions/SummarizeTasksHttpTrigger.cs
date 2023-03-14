using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TaskSummarizer.Shared.Models;
using TaskSummarizer.Shared.Services;
using static TaskSummarizer.Shared.Helpers.OpenAiHelpers;

namespace TasksSummarizer.Functions.Functions
{
    public class SummarizeTasksHttpTrigger
    {
        private readonly ILogger _logger;

        public SummarizeTasksHttpTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SummarizeTasksHttpTrigger>();
        }

        [Function("SummarizeTasksHttpTrigger")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            // get tasksSummary from json body
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var items = JsonConvert.DeserializeObject<List<TaskItem>>(requestBody);

            HttpResponseData? response;

            if (items is null || items.Count == 0)
            {
                response = req.CreateResponse(HttpStatusCode.BadRequest);
                await response.WriteStringAsync("Please pass a list of task items");
                return response;
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

            var filePath = Path.Combine(Environment.CurrentDirectory, "Prompts", "SummarizeText.txt");
            var baseSystemMessage = await File.ReadAllTextAsync(filePath);

            baseSystemMessage = baseSystemMessage.Replace("Peter Parker", "Steven Strange");

            var chatService = new OpenAiChatService(apiKey, baseUrl, deploymentId);
            var prompt = GetPromptFromTasks(items, baseSystemMessage);
            var openAiResponse = await chatService.CreateCompletionAsync(prompt);

            response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync(openAiResponse?.Choices?.FirstOrDefault()?.Text ?? "Nothing to show");

            return response;
        }
    }
}
