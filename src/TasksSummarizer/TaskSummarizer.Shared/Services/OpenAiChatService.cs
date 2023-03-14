using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using TaskSummarizer.Shared.Models;
using static TaskSummarizer.Shared.Helpers.OpenAiHelpers;

namespace TaskSummarizer.Shared.Services
{
    public class OpenAiChatService

    {
        private string ApiKey { get; }
        private string BaseUrl { get; }
        private HttpDataService HttpDataService { get; }
        
        
        public OpenAiChatService(string apiKey, string baseUrl)
        {
            ApiKey = apiKey;
            BaseUrl = baseUrl;

            var endpointUrl = $"https://{baseUrl}/openai/deployments/gpt-35-turbo/completions?api-version=2022-12-01";
            HttpDataService = new HttpDataService(endpointUrl);
        }

        public string GetPromptFromTasks(List<TaskItem> taskItems, string fullName)
        {
            var tasks = JsonConvert.SerializeObject(taskItems, Formatting.Indented);

            var systemMessage = GetSummarizerSystemMessage(fullName);

            const string intro = "Here are the tasks done, generate the summary in 2-4 bullet points, in prose format:";
            var serializedTasks = JsonConvert.SerializeObject(taskItems, Formatting.Indented);

            var userMessage = new Dictionary<string, string>()
            {
                { "sender", "user" },
                { "text", $"{intro}\n\n{serializedTasks}" }
            };

            var prompt = CreatePrompt(userMessage, systemMessage);

            return prompt;
        }

        public async Task<OpenAiResponse?> CreateCompletionAsync(string prompt)
        {
            var completion = new OpenAiCompletion()
            {
                Prompt = prompt,
                Temperature = 1,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
                MaxTokens = 1000,
                TopP = 0.95
            };

            var response = await HttpDataService.PostAsJsonAsync<OpenAiCompletion>("", completion, "api-key", ApiKey);

            if (response is OpenAiResponse openAiResponse)
            {
                return openAiResponse;
            }

            else
            {
                return null;
            }
        }
    }
}
