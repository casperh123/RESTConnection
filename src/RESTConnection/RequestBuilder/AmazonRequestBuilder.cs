using System.Text;
using DataDownloader.Connection.Authentication;
using DataDownloader.Connection.RESTConnection.RequestBuilder.Url;
using Newtonsoft.Json;

namespace DataDownloader.Connection.RESTConnection.RequestBuilder
{
    public class AmazonRequestBuilder : AbstractRequestBuilder
    {
        private readonly IAuthentication _authentication;
        private readonly Region _region;
        private static readonly string DateFormat = "yyyyMMddTHHmmssZ";
        private static readonly string UserAgent = "Data Retrieval/1.0 (Language=C#)";
        private static readonly Dictionary<Region, string> RegionHostMapping = new()
        {
            { Region.NorthAmerica, "sellingpartnerapi-na.amazon.com" },
            { Region.Europe, "sellingpartnerapi-eu.amazon.com" },
            { Region.FarEast, "sellingpartnerapi-fe.amazon.com" },
            { Region.Sandbox, "sandbox.sellingpartnerapi-eu.amazon.com" }
        };

        public AmazonRequestBuilder(IAuthentication authentication, Region region) : base(new RequestUrl(RegionHostMapping[region]))
        {
            _authentication = authentication ?? throw new ArgumentNullException(nameof(authentication));
            _region = region;
        }

        protected override void AddRequiredHeaders(HttpRequestMessage request)
        {
            request.Headers.Add("x-amz-access-token", _authentication.HeaderFormat());
            request.Headers.Add("x-amz-date", DateTime.UtcNow.ToString(DateFormat));
            request.Headers.Add("host", RegionHostMapping[_region]);
            request.Headers.UserAgent.ParseAdd(UserAgent);
        }
    }

    public enum Region
    {
        NorthAmerica,
        Europe,
        FarEast,
        Sandbox
    }
}
