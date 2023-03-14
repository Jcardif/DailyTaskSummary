using Newtonsoft.Json;

namespace TaskSummarizer.Shared.Models
{
    public class OpenAiCompletion
    {
        [JsonProperty("prompt")]
        public string? Prompt { get; set; }

        [JsonProperty("temperature")]
        public long Temperature { get; set; }

        [JsonProperty("top_p")]
        public double TopP { get; set; }

        [JsonProperty("frequency_penalty")]
        public long FrequencyPenalty { get; set; }

        [JsonProperty("presence_penalty")]
        public long PresencePenalty { get; set; }

        [JsonProperty("max_tokens")]
        public long MaxTokens { get; set; }

        [JsonProperty("stop")]
        public List<string> Stop { get; set; } = new()
        {
            "<|im_end|>"
        };
    }
}
