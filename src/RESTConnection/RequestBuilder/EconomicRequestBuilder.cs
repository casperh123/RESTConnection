using System.Text;
using DataDownloader.Connection.Authentication;
using DataDownloader.Connection.RESTConnection.RequestBuilder.Url;
using Newtonsoft.Json;

namespace DataDownloader.Connection.RESTConnection.RequestBuilder;

public class EconomicRequestBuilder : AbstractRequestBuilder
{
    private readonly IAuthentication _authentication;

    public EconomicRequestBuilder(IAuthentication authentication) : base(new RequestUrl("restapi.e-conomic.com"))
    {
        _authentication = authentication ?? throw new ArgumentNullException(nameof(authentication));
    }
    
    protected override void AddRequiredHeaders(HttpRequestMessage request)
    {
        request.Headers.Add("X-AppSecretToken", _authentication.ClientId);
        request.Headers.Add("X-AgreementGrantToken", _authentication.HeaderFormat());
    }
}