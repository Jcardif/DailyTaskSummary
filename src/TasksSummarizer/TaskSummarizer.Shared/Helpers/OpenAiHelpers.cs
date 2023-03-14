namespace TaskSummarizer.Shared.Helpers
{
    public static class OpenAiHelpers
    {
        /// <summary>
        ///     Get the system message
        /// </summary>
        /// <param name="baseSystemMessage"></param>
        /// <returns></returns>
        public static string GetSummarizerSystemMessage(string baseSystemMessage)
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
