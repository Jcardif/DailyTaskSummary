using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TaskSummarizer.Shared.Models;
using TaskSummarizer.Shared.Services;

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
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            // Get settings from local.setting
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var apiKey = config.GetValue<string>("AzureOpenAI:APIKey");
            var resourceName = config.GetValue<string>("AzureOpenAI:ResourceName");
            var deploymentId = config.GetValue<string>("AzureOpenAI:DeploymentId");
            var baseUrl = config.GetValue<string>("AzureOpenAI:BaseUrl");

            var items = new List<TaskItem>()
            {
                new TaskItem()
                {
                    Description = "",
                    Importance = "normal",
                    Status = "notStarted",
                    SubTasks = new List<TaskItemSubTask>
                    {
                        new TaskItemSubTask()
                        {
                            DisplayName = "respond to E",
                            SubTaskStatus = "completed"
                        },
                        new TaskItemSubTask()
                        {
                            DisplayName = "check for additional intesting topics",
                            SubTaskStatus = "completed"
                        }
                    },
                    TaskTitle = "Check watercooler moderation"
                },
                new TaskItem()
                {
                    Description = "",
                    Importance = "normal",
                    Status = "notStarted",
                    SubTasks = new List<TaskItemSubTask>
                    {
                        new TaskItemSubTask()
                        {
                            DisplayName = "design powerbi dashboards",
                            SubTaskStatus = "completed"
                        },
                        new TaskItemSubTask()
                        {
                            DisplayName = "review what goes to osdc blogs",
                            SubTaskStatus = "in progress"
                        }
                    },
                    TaskTitle = "Synapse Blog 2"
                }
            };

            var chatService = new OpenAiChatService(apiKey, baseUrl, deploymentId);
            var prompt = chatService.GetPromptFromTasks(items, "Peter Parker");
            var openAiResponse = await chatService.CreateCompletionAsync(prompt);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync(openAiResponse?.Choices?.FirstOrDefault()?.Text ?? "Nothing to show");

            return response;
        }
    }
}
