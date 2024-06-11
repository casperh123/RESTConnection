using RESTConnection.Connection.RequestBuilder;
using System.Net.Http.Headers;

namespace RESTConnection.Connection
{
    public class APIConnection : IRESTConnection
    {
        private readonly HttpClient _client;
        private readonly IRequestBuilder _requestBuilder;
        private DateTime? _lastForbiddenResponseTime;

        public int InitialRetryDelay { get; set; } = 60; // default initial retry delay in seconds
        public int MaxRetries { get; set; } = 5; // maximum number of retries

        public APIConnection(IRequestBuilder requestBuilder, HttpClient httpClient)
        {
            _client = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _requestBuilder = requestBuilder ?? throw new ArgumentNullException(nameof(requestBuilder));
        }

        public async Task<HttpResponseMessage> RequestAsync(HttpMethod method, string endpoint, object content, params Parameter[] parameters)
        {
            HttpResponseMessage response = null;
            int retries = 0;
            bool shouldRetry;

            var request = _requestBuilder.BuildRequest(method, endpoint, content, parameters);

            do
            {
                shouldRetry = false;

                await CheckAndApplyDelay();

                // Clone the request message to avoid issues with reusing it
                var requestClone = CloneHttpRequestMessage(request);

                response = await _client.SendAsync(requestClone, HttpCompletionOption.ResponseHeadersRead);

                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    _lastForbiddenResponseTime = DateTime.UtcNow;
                    var retryAfter = response.Headers.RetryAfter?.Delta?.TotalSeconds ?? InitialRetryDelay;
                    shouldRetry = retries < MaxRetries;
                    retries++;
                    if (shouldRetry)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(retryAfter));
                    }
                }
            } while (shouldRetry);

            return response;
        }

        private async Task CheckAndApplyDelay()
        {
            if (_lastForbiddenResponseTime.HasValue)
            {
                var timeSinceLastForbidden = DateTime.UtcNow.Subtract(_lastForbiddenResponseTime.Value).TotalSeconds;
                if (timeSinceLastForbidden < InitialRetryDelay)
                {
                    await Task.Delay(TimeSpan.FromSeconds(InitialRetryDelay - timeSinceLastForbidden));
                }
            }
        }

        private HttpRequestMessage CloneHttpRequestMessage(HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Content = request.Content,
                Version = request.Version
            };

            foreach (var header in request.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            foreach (var property in request.Options)
            {
                clone.Options.TryAdd(property.Key, property.Value);
            }

            return clone;
        }
    }
}
