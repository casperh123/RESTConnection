using Newtonsoft.Json.Linq;

namespace DataDownloader.Connection.RESTConnection
{
    public static class TokenRequests
    {
        public static async Task<string> GetAmazonAccessGetAccessToken(string clientId, string clientSecret, string refreshToken)
        {
            HttpClient client = HttpClientSingleton.HttpClientSingleton.Instance;
            
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
            
            JObject json = JObject.Parse(await response.Content.ReadAsStringAsync());
            return json["access_token"]?.ToString();
        }
    }
}