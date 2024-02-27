using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace RESTConnection.Authentication.TokenRequest
{
    public class AmazonTokenRequestService : ITokenRequestService
    {
        private readonly HttpClient _client;

        public AmazonTokenRequestService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<string> GetAccessToken(string clientId, string clientSecret, string refreshToken)
        {
            Dictionary<string, string> requestHeaders = new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = refreshToken,
                ["client_id"] = clientId,
                ["client_secret"] = clientSecret
            };

            HttpResponseMessage response = await _client.PostAsync(
                "https://api.amazon.com/auth/o2/token",
                new FormUrlEncodedContent(requestHeaders)
            );

            response.EnsureSuccessStatusCode();

            JsonElement json = await JsonSerializer.DeserializeAsync<JsonElement>(await response.Content.ReadAsStreamAsync());

            if (json.TryGetProperty("access_token", out JsonElement accessTokenElement))
            {
                return accessTokenElement.ToString();
            }
            else
            {
                throw new InvalidOperationException("Access token not found in the response.");
            }
        }
    }
}