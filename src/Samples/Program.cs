using Newtonsoft.Json;
using TiktokenSharp;

namespace Samples
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var systemMessage = GetSystemMessage("Josh N");
            var oaiChatGpt = new OAIChatGPT(systemMessage);

            string jsonData = @"[
              {
                ""Description"": """",
                ""Importance"": ""normal"",
                ""Status"": ""notStarted"",
                ""SubTasks"": [
                  {
                    ""displayName"": ""respond to E"",
                    ""subTaskStatus"": ""completed""
                  },
                  {
                    ""displayName"": ""check for additional intesting topics"",
                    ""subTaskStatus"": ""completed""
                  }
                ],
                ""TaskTitle"": ""Check watercooler moderation""
              },
              {
                ""Description"": """",
                ""Importance"": ""normal"",
                ""Status"": ""notStarted"",
                ""SubTasks"": [
                  {
                    ""displayName"": ""design powerbi dashboards"",
                    ""subTaskStatus"": ""completed""
                  },
                  {
                    ""displayName"": ""review what goes to osdc blogs"",
                    ""subTaskStatus"": ""in progress""
                  }
                ],
                ""TaskTitle"": ""Synapse Blog 2""
              }
            ]";


            var messages = new Dictionary<string,string>()
            {
                {"sender", "user"},
                {"text", jsonData}
            };

            var prompt= oaiChatGpt.CreatePrompt(messages);


            var GptBody = new GptBody()
            {
                Prompt = prompt,
                Temperature = 0,
                TopP = 1,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
                MaxTokens = 100,
                Stop = null
            };

            var str = JsonConvert.SerializeObject(GptBody);
            Console.WriteLine(str);
            
        }

        public static string GetSystemMessage(string fullName)
        {
            var baseSystemMessage =  string.Join("\n",
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





    }

    public class GptBody
    {
        [JsonProperty("prompt")]
        public string Prompt { get; set; }

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
        public object Stop { get; set; }

    }

    public class OAIChatGPT
    {
        public string EndPoint {get;set;}
        public string Location {get;set;}
        public string SystemMessage {get;set;}
        public string Prompt {get;set;}
    

        public OAIChatGPT(string endPoint, string location, string systemMessage)
        {
            EndPoint = endPoint;
            Location = location;
            SystemMessage = systemMessage;
        }

        public OAIChatGPT(string systemMessage)
        {
            SystemMessage = systemMessage;
        }


        /// <summary>
        ///     Create a prompt from the system message and messages.
        /// </summary>
        /// <param name="systemMessage">The system message.</param>
        /// <param name="messages">
        ///     The list of messages, each represented as a dynamic object with "sender" and "text" keys.
        ///     Example: messages = [{"sender": "user", "text": "I want to write a blog post about my company."}]
        ///</param>
        /// <returns>The prompt string.</returns>
        public string CreatePrompt(dynamic message)
        {
            var prompt = SystemMessage;

            prompt += $"\n<|im_start|>{message["sender"]}\n{message["text"]}<|im_end|>";
            prompt += "\n<|im_start|>assistant\n";

            return prompt;
        }
    }

    public class TaskItems
    {
        public string Description { get; set; }
        public string Importance { get; set; }
        public string Status { get; set; }
        public List<SubTask> SubTasks { get; set; }
        public string TaskTitle { get; set; }
    }

    public partial class SubTask
    {
        public string DisplayName { get; set; }
        public string SubTaskStatus { get; set; }
    }
}
