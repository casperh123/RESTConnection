using System.Text.Json;

namespace RESTConnection.Authentication.TokenRequest
{
    public static class TokenRequests
    {
        public static async Task<string> GetAmazonAccessToken(string clientId, string clientSecret, string refreshToken)
        {
            HttpClient client = HttpClientSingleton.Instance; // Assuming HttpClientSingleton is correctly implemented

            Dictionary<string, string> requestHeaders = new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = refreshToken,
                ["client_id"] = clientId,
                ["client_secret"] = clientSecret
            };

            HttpResponseMessage response = await client.PostAsync(
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