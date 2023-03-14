using System;
using System.Collections.Generic;
using System.Text;

namespace TaskSummarizer.Shared.Services
{
    public class HttpDataService
    {
        private readonly HttpClient client;
        private readonly Dictionary<string, object> responseCache;

        public HttpDataService(string defaultBaseUrl)
        {

            client = new HttpClient();

            if (!string.IsNullOrEmpty(defaultBaseUrl))
                client.BaseAddress = defaultBaseUrl.EndsWith("/")
                    ? new Uri(defaultBaseUrl)
                    : new Uri($"{defaultBaseUrl}/");

            responseCache = new Dictionary<string, object>();
        }

        public async Task<T> GetAsync<T>(string uri, string accessToken = null,
            List<(string name, List<string> value)> headers = null, bool forceRefresh = false)
        {
            T result = default;

            // If url ends with a slash remove the slash
            if (uri.EndsWith("/"))
                uri = uri.Remove(uri.Length - 1);

            // If header parameters are provided append them to the url
            if (headers != null)
            {
                foreach (var header in headers) client.DefaultRequestHeaders.Add(header.name, header.value);

                // Append the ? to begin the header parameters sequence
                uri = $"{uri}?";

                // Loop through the parameters and append them to the url
                for (var i = 0; i < headers.Count; i++)
                {
                    if (i < 1)
                        uri += $"{headers[i].name}={headers[i].value}";
                    if (i >= 1)
                        uri += $"&{headers[i].name}={headers[i].value}";
                }
            }

            // The responseCache is a simple store of past responses to avoid unnecessary requests for the same resource.
            // Feel free to remove it or extend this request logic as appropraite for your app.
            if (forceRefresh || !responseCache.ContainsKey(uri))
            {
                if (accessToken != null)
                    AddAuthorizationHeader(accessToken);

                var response = await client.GetAsync(uri);

                if (!response.IsSuccessStatusCode)
                    return default;

                var json = await response.Content.ReadAsStringAsync();
                result = await Task.Run(() => JsonConvert.DeserializeObject<T>(json));

                if (responseCache.ContainsKey(uri))
                    responseCache[uri] = result;
                else
                    responseCache.Add(uri, result);
            }
            else
            {
                result = (T)responseCache[uri];
            }

            return result;
        }

        public async Task<bool> PostAsync<T>(string uri, T item)
        {
            if (item == null) return false;

            var serializedItem = JsonConvert.SerializeObject(item);
            var buffer = Encoding.UTF8.GetBytes(serializedItem);
            var byteContent = new ByteArrayContent(buffer);

            var response = await client.PostAsync(uri, byteContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<object> PostAsJsonAsync<T>(string uri, T item, string accessToken)
        {
            if (item == null)
                return null;
            AddAuthorizationHeader(accessToken);

            var serializedItem = await Task.Run(() => JsonConvert.SerializeObject(item));
            var stringContent = new StringContent(serializedItem, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(uri, stringContent);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.Created)
                return await Json.ToObjectAsync<T>(json);

            return await Json.ToObjectAsync<object>(json);

            //TODO : Handle other status codes
        }


        public async Task<bool> PutAsync<T>(string uri, T item)
        {
            if (item == null) return false;

            var serializedItem = JsonConvert.SerializeObject(item);
            var buffer = Encoding.UTF8.GetBytes(serializedItem);
            var byteContent = new ByteArrayContent(buffer);

            var response = await client.PutAsync(uri, byteContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PutAsJsonAsync<T>(string uri, T item, string accessToken)
        {
            if (item == null) return false;

            AddAuthorizationHeader(accessToken);
            var serializedItem = JsonConvert.SerializeObject(item);

            var response =
                await client.PutAsync(uri, new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PutAsJsonAsync(string uri, string accessToken)
        {
            AddAuthorizationHeader(accessToken);

            var response =
                await client.PutAsync(uri, null);

            return response.IsSuccessStatusCode;
        }


        public async Task<bool> DeleteAsync(string uri)
        {
            var response = await client.DeleteAsync(uri);

            return response.IsSuccessStatusCode;
        }

        // Add this to all public methods
        private void AddAuthorizationHeader(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = null;
                return;
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
