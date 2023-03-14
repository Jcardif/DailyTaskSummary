using Newtonsoft.Json;
using TaskSummarizer.Shared.Models;

namespace TaskSummarizer.Shared.Helpers
{
    public static class OpenAiHelpers
    {
        /// <summary>
        ///     Get the system message
        /// </summary>
        /// <param name="baseSystemMessage"></param>
        /// <returns></returns>
        private static string GetSystemMessage(string baseSystemMessage)
        {
            var systemMessage = $"<|im_start|>system\n{baseSystemMessage.Trim()}\n<|im_end|>";
            return systemMessage;
        }

        /// <summary>
        ///     Create a prompt from the system message and messages.
        /// </summary>
        /// <param name="systemMessage">The system message.</param>
        /// <param name="message">
        ///     The list of messages, each represented as a dynamic object with "sender" and "text" keys.
        ///     Example: messages = [{"sender": "user", "text": "I want to write a blog post about my company."}]
        ///</param>
        /// <returns>The prompt string.</returns>
        private static string CreatePrompt(dynamic message, string systemMessage)
        {
            var prompt = systemMessage;

            prompt += $"\n<|im_start|>{message["sender"]}\n{message["text"]}<|im_end|>";
            prompt += "\n<|im_start|>assistant\n";

            return prompt;
        }


        public static string GetPromptFromTasks(List<TaskItem> taskItems, string baseSystemMessage)
        {
            var tasks = JsonConvert.SerializeObject(taskItems, Formatting.Indented);

            var systemMessage = GetSystemMessage(baseSystemMessage);

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

        public static string GetAdaptiveCardPrompt(string tasksSummary, string baseSystemMessage)
        {
            var systemMessage = GetSystemMessage(baseSystemMessage);

            const string intro = "Here is the summary of the work done";

            var userMessage = new Dictionary<string, string>()
            {
                { "sender", "user" },
                { "text", $"{intro}\n\n{tasksSummary}" }
            };

            var prompt = CreatePrompt(userMessage, systemMessage);

            return prompt;
        }

        // Todo: Estimate number of tokens in a prompt

    }
}
