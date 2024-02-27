using System.Net;

namespace RESTConnection.Connection.RequestBuilder.Url;

public class RequestUrl
{
    private readonly string _url;
    
    public RequestUrl(string url)
    {
        _url = url ?? throw new ArgumentNullException(nameof(url));
    }

    public string GetRequestUrl(string endpoint, params Parameter[] parameters)
    {
        if (string.IsNullOrWhiteSpace(endpoint))
        {
            throw new ArgumentNullException(nameof(endpoint));
        }

        UriBuilder uriBuilder = new UriBuilder()
        {
            Host = _url,
            Path = endpoint,
            Scheme = "https"
        };
        
        List<string> queries = new();

        IEnumerable<IGrouping<string, Parameter>> groupedParameters = parameters.GroupBy(p => p.Key);

        foreach (IGrouping<string, Parameter> group in groupedParameters)
        {
            string key = WebUtility.UrlEncode(group.Key);
            IEnumerable<string> values = group.Select(p => WebUtility.UrlEncode(p.Value));
            string valueString = string.Join(",", values);

            queries.Add($"{key}={valueString}");
        }

        uriBuilder.Query = string.Join("&", queries);

        return uriBuilder.ToString();
    }
}