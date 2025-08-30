namespace FirstOpenAIconsole
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    static class PerplexityChat
    {
        private static readonly HttpClient client = new HttpClient();
        private static List<dynamic> conversation = new List<dynamic>();

        public static async Task Main()
        {
            string apiKey = "pplx-***"; // Replace with your actual API key
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            // Initialize the conversation with system prompt or empty
            conversation.Add(new { role = "system", content = "You are a helpful assistant." });

            while (true)
            {
                Console.Write("User: ");
                string userInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(userInput) || userInput == "exit" || userInput.Contains("stop"))
                    break;

                // Add user message to conversation
                conversation.Add(new { role = "user", content = userInput });

                // Create request body with full conversation
                var requestBody = new { temperature = 0.5, model = "sonar-pro", messages = conversation, max_tokens = 300 };

                string jsonBody = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://api.perplexity.ai/chat/completions", content);
                ////var response = await client.PostAsync("https://api.perplexity.ai/async/chat/completions", content);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                // Parse response (adjust parsing based on actual API response format)
                using JsonDocument doc = JsonDocument.Parse(responseBody);
                string assistantReply = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

                Console.WriteLine("Assistant: " + assistantReply);

                // Add assistant message to conversation
                conversation.Add(new { role = "assistant", content = assistantReply });
            }
        }
    }
}