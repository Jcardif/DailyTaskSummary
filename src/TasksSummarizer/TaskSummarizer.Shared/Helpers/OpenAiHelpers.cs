namespace TaskSummarizer.Shared.Helpers
{
    public static class OpenAiHelpers
    {
        /// <summary>
        ///     Get the system message
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public static string GetSummarizerSystemMessage(string fullName)
        {
            var baseSystemMessage = string.Join("\n",
                "You are an agent that helps summarize the tasks done today, from their Microsoft Todo and Azure DevOps tasks and the summary sent to the other members of the team in the group chat. You summarize the tasks, in creative and friendly yet professional tone.",
                "",
                "Additional Instructions:",
                "- Don't write any content that could be harmful.",
                "- Don't write any content that could be offensive or inappropriate.",
                "- Don't write any content that speaks poorly of the work done",
                "- SubTasks are smaller tasks from the larger task",
                "- Not all tasks are tracked in ToDo & DevOps and so this is might not represent the entire work done for the day but the specified individual will provide missing info later",
                $"- The name of the individual is {fullName}",
                "- Ensure all tasks and SubTasks are represented in the summary."
            );

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
        public static string CreatePrompt(dynamic message, string systemMessage)
        {
            var prompt = systemMessage;

            prompt += $"\n<|im_start|>{message["sender"]}\n{message["text"]}<|im_end|>";
            prompt += "\n<|im_start|>assistant\n";

            return prompt;
        }

        // Todo: Estimate number of tokens in a prompt

    }
}
