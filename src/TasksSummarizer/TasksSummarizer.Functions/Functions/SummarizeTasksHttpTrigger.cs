using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
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
            HttpResponseData? response;

            // get name of the user sending tasks
            var uri = new Uri(req.Url.ToString());
            var query = QueryHelpers.ParseQuery(uri.Query);

            if (!query.ContainsKey("name") || string.IsNullOrEmpty(query["name"]))
            {
                var error = new { error = "Please pass name in the query string" };
                response = req.CreateResponse(HttpStatusCode.BadRequest);
                await response.WriteAsJsonAsync(error);

                return response;
            }

            var name = query["name"];

            // get tasksSummary from json body
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var items = JsonConvert.DeserializeObject<List<TaskItem>>(requestBody);

            if (items is null || items.Count == 0)
            {
                var error = new { error = "Please pass valid tasks in  the request body" };
                
                response = req.CreateResponse(HttpStatusCode.BadRequest);
                await response.WriteAsJsonAsync(error);
                
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

            baseSystemMessage = baseSystemMessage.Replace("Peter Parker", name);

            var chatService = new OpenAiChatService(apiKey, baseUrl, deploymentId);
            var prompt = GetPromptFromTasks(items, baseSystemMessage);
            var openAiResponse = await chatService.CreateCompletionAsync(prompt);

            var summary = new { taskSummary = openAiResponse?.Choices?.FirstOrDefault()?.Text ?? "" };

           response = req.CreateResponse(HttpStatusCode.OK);
           await response.WriteAsJsonAsync(summary);   

            return response;
        }
    }
}
