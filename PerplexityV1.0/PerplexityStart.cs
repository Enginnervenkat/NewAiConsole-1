using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

class PerplexityStart
{
    private static readonly HttpClient client = new HttpClient();

    public static async Task CallPerplexityApiAsync(string apiKey, string prompt)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var requestBody = new
        {
            model = "sonar-pro",  // or other appropriate model name
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        string jsonBody = System.Text.Json.JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        try
        {
            var response = await client.PostAsync("https://api.perplexity.ai/chat/completions", content);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Response:");
            Console.WriteLine(responseBody);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error:");
            Console.WriteLine(ex.Message);
        }
    }

    public static async Task Main()
    {
        string apiKey = "pplx-***";
        string prompt = "Explain me about AI.";

        await CallPerplexityApiAsync(apiKey, prompt);
    }
}
