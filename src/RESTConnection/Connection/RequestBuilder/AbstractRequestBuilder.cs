using System.Text;
using System.Text.Json;
using RESTConnection.Authentication;
using RESTConnection.Connection.RequestBuilder.Url;

namespace RESTConnection.Connection.RequestBuilder;

public abstract class AbstractRequestBuilder : IRequestBuilder
{
    private readonly RequestUrl _requestUrl;
    private readonly IAuthentication _authentication;

    protected AbstractRequestBuilder(RequestUrl requestUrl, IAuthentication authentication)
    {
        _requestUrl = requestUrl ?? throw new ArgumentNullException(nameof(requestUrl), "RequestUrl cannot be null.");
        _authentication = authentication ?? throw new ArgumentNullException(nameof(authentication), "Authentication cannot be null.");
    }

    public HttpRequestMessage BuildRequest(HttpMethod method, string endpoint, object content = null, params Parameter[] parameters)
    {
        
        if (string.IsNullOrWhiteSpace(endpoint))
            throw new ArgumentNullException(nameof(endpoint), "Endpoint cannot be null or empty.");

        HttpRequestMessage request = new HttpRequestMessage
        {
            Method = method,
            RequestUri = new Uri(_requestUrl.GetRequestUrl(endpoint, parameters)),
            Content = content != null ? new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json") : null
        };
        
        AddAuthenticationHeaders(request, _authentication.AuthenticationHeaders());
        AddRequiredHeaders(request);

        return request;
    }

    private void AddAuthenticationHeaders(HttpRequestMessage request, Dictionary<string, string> customHeaders)
    {
        foreach (KeyValuePair<string, string> header in customHeaders)
        {
            request.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }
    }

    protected abstract void AddRequiredHeaders(HttpRequestMessage request);
}