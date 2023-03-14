using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAI_API;
using OpenAI_API.Chat;

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


            OpenAIAPI api = OpenAIAPI.ForAzure(apiKey, resourceName, deploymentId);

            var chat = api.Chat.CreateConversation();

            /// give instruction as System
            chat.AppendSystemMessage("You are a teacher who helps children understand if things are animals or not.  If the user tells you an animal, you say \"yes\".  If the user tells you something that is not an animal, you say \"no\".  You only ever respond with \"yes\" or \"no\".  You do not say anything else.");

            // give a few examples as user and assistant
            chat.AppendUserInput("Is this an animal? Cat");
            chat.AppendExampleChatbotOutput("Yes");
            chat.AppendUserInput("Is this an animal? House");
            chat.AppendExampleChatbotOutput("No");

            // now let's ask it a question'
            chat.AppendUserInput("Is this an animal? Dog");
            // and get the response
            string response = await chat.GetResponseFromChatbot();
            _logger.LogInformation(response); // "Yes"

            // and continue the conversation by asking another
            chat.AppendUserInput("Is this an animal? Chair");
            // and get another response
            response = await chat.GetResponseFromChatbot();
            _logger.LogInformation(response); // "No"

            // the entire chat history is available in chat.Messages
            foreach (ChatMessage msg in chat.Messages)
            {
                Console.WriteLine($"{msg.Role}: {msg.Content}");
            }
            

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
