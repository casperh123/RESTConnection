using RESTConnection.Connection.RequestBuilder;

namespace RESTConnection.Connection
{
    public class RESTConnection(IRequestBuilder requestBuilder, HttpClient httpClient) : IRESTConnection
    {
        private readonly HttpClient _client = httpClient;
        private readonly IRequestBuilder _requestBuilder = requestBuilder ?? throw new ArgumentNullException(nameof(requestBuilder));
        private DateTime? _lastForbiddenResponseTime;
        
        public int RetryDelay { get; set; } = 60;

        public async Task<HttpResponseMessage> RequestAsync(HttpMethod method, string endpoint, object content, params Parameter[] parameters)
        {
            HttpResponseMessage response = null;
            bool shouldRetry;

            do
            {
                shouldRetry = false;

                await CheckAndApplyDelay();

                HttpRequestMessage request = _requestBuilder.BuildRequest(method, endpoint, content, parameters);
                response = await _client.SendAsync(request);
                
                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    _lastForbiddenResponseTime = DateTime.UtcNow;
                    shouldRetry = true;
                }
            } while (shouldRetry);

            return response;
        }
        
        private async Task CheckAndApplyDelay()
        {
            if (_lastForbiddenResponseTime.HasValue)
            {
                var timeSinceLastForbidden = DateTime.UtcNow.Subtract(_lastForbiddenResponseTime.Value).TotalSeconds;
                if (timeSinceLastForbidden <= RetryDelay)
                {
                    await Task.Delay(TimeSpan.FromSeconds(RetryDelay - timeSinceLastForbidden));
                }
            }
        }
    }
}