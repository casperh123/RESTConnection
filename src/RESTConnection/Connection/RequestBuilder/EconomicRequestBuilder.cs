using RESTConnection.Authentication;
using RESTConnection.Connection.RequestBuilder.Url;

namespace RESTConnection.Connection.RequestBuilder;

public class EconomicRequestBuilder(IAuthentication authentication) : AbstractRequestBuilder(new RequestUrl("restapi.e-conomic.com"), authentication)
{
    protected override void AddRequiredHeaders(HttpRequestMessage request)
    {
    }
}