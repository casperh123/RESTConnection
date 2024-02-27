using System.Text;
using DataDownloader.Connection.RESTConnection.RequestBuilder.Url;
using Newtonsoft.Json;

namespace DataDownloader.Connection.RESTConnection.RequestBuilder;

public abstract class AbstractRequestBuilder : IRequestBuilder
{
    private RequestUrl _requestUrl;

    protected AbstractRequestBuilder(RequestUrl requestUrl)
    {
        _requestUrl = requestUrl;
    }

    public HttpRequestMessage BuildRequest(HttpMethod method, string endpoint, object content = null, params Parameter[] parameters)
    {
        
        if (string.IsNullOrWhiteSpace(endpoint))
            throw new ArgumentNullException(nameof(endpoint), "Endpoint cannot be null or empty.");

        HttpRequestMessage request = new HttpRequestMessage
        {
            Method = method,
            RequestUri = new Uri(_requestUrl.GetRequestUrl(endpoint, parameters)),
            Content = content != null ? new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json") : null
        };

        AddRequiredHeaders(request);

        return request;
    }

    protected abstract void AddRequiredHeaders(HttpRequestMessage request);
}