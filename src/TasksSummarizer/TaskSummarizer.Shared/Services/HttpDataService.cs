using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace TaskSummarizer.Shared.Services
{
    public class HttpDataService
    {
        private readonly HttpClient _client;

        public HttpDataService(string defaultBaseUrl)
        {

            _client = new HttpClient();

            if (!string.IsNullOrEmpty(defaultBaseUrl))
                _client.BaseAddress = defaultBaseUrl.EndsWith("/")
                    ? new Uri(defaultBaseUrl)
                    : new Uri($"{defaultBaseUrl}/");
        }

        

        public async Task<T?> GetAsync<T>(string uri, string? scheme = null, string? parameter = null, List<(string name, List<string> value)>? headers = null, bool forceRefresh = false)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            AddAuthorizationHeader(scheme, parameter);

            if (headers != null)
                foreach (var header in headers)
                    request.Headers.Add(header.name, header.value);

            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }

            throw new HttpRequestException($"Error calling {uri} - {response.StatusCode}");
        }

        public async Task<object?> PostAsJsonAsync<T>(string uri, T item, string? scheme = null, string? parameter =null, List<(string name, List<string> value)>? headers = null, bool forceRefresh = false)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);

            AddAuthorizationHeader(scheme, parameter);

            if (headers != null)
                foreach (var header in headers)
                    request.Headers.Add(header.name, header.value);

            request.Content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject(content);
            }

            throw new HttpRequestException($"Error calling {uri} - {response.StatusCode}");
        }
        
        


        // Add this to all public methods
        private void AddAuthorizationHeader(string? scheme, string? parameter)
        {
            if (string.IsNullOrEmpty(scheme) || string.IsNullOrEmpty(parameter))
            {
                _client.DefaultRequestHeaders.Authorization = null;
                return;
            }

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, parameter);
        }
    }

}
