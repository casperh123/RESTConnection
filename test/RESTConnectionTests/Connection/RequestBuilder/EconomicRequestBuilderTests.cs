using RESTConnection.Authentication;
using RESTConnection.Connection.RequestBuilder;
using System.Net.Http;
using Xunit;

public class EconomicRequestBuilderTests
{
    [Fact]
    public void Constructor_SetsBaseUrlCorrectly()
    {
        IAuthentication mockAuthentication = new MockAuthentication();
        
        EconomicRequestBuilder builder = new EconomicRequestBuilder(mockAuthentication);
        HttpRequestMessage request = builder.BuildRequest(HttpMethod.Get, "/customers");
        
        Assert.NotNull(request.RequestUri);
        Assert.Equal("https://restapi.e-conomic.com/customers", request.RequestUri.ToString());
    }

    [Fact]
    public void BuildRequest_UsesAuthenticationHeaders()
    {
        IAuthentication mockAuthentication = new MockAuthentication();
        IRequestBuilder builder = new EconomicRequestBuilder(mockAuthentication);
        
        HttpRequestMessage request = builder.BuildRequest(HttpMethod.Get, "/customers");
        
        Assert.True(request.Headers.Contains("Authorization"));
        Assert.Contains("Bearer example-token", request.Headers.Authorization.ToString());
    }
    
    private class MockAuthentication : IAuthentication
    {
        public Dictionary<string, string> AuthenticationHeaders() => new Dictionary<string, string>
        {
            { "Authorization", "Bearer example-token" }
        };
    }
}
