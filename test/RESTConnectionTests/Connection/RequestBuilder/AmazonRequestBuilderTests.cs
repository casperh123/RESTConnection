using System;
using System.Net.Http;
using System.Linq;
using Xunit;
using RESTConnection.Authentication;
using RESTConnection.Connection.RequestBuilder;

public class AmazonRequestBuilderTests
{
    // Mock IAuthentication for use in tests
    private readonly IAuthentication _mockAuthentication = new MockAuthentication();

    [Theory]
    [InlineData(Region.NorthAmerica, "sellingpartnerapi-na.amazon.com")]
    [InlineData(Region.Europe, "sellingpartnerapi-eu.amazon.com")]
    [InlineData(Region.FarEast, "sellingpartnerapi-fe.amazon.com")]
    [InlineData(Region.Sandbox, "sandbox.sellingpartnerapi-eu.amazon.com")]
    public void BuildRequest_CreatesRequestWithCorrectHostBasedOnRegion(Region region, string expectedHost)
    {
        // Arrange
        var builder = new AmazonRequestBuilder(_mockAuthentication, region);
        
        // Act
        var request = builder.BuildRequest(HttpMethod.Get, "testEndpoint"); // Assuming BuildRequest is accessible and returns HttpRequestMessage

        // Assert
        Assert.NotNull(request.RequestUri);
        Assert.Equal(expectedHost, request.RequestUri.Host);
    }

    [Fact]
    public void BuildRequest_AddsRequiredHeaders()
    {
        // Arrange
        var region = Region.NorthAmerica; // Example region
        var builder = new AmazonRequestBuilder(_mockAuthentication, region);
        
        // Act
        var request = builder.BuildRequest(HttpMethod.Get, "testEndpoint"); // Assuming BuildRequest is accessible and returns HttpRequestMessage

        // Assert
        Assert.True(request.Headers.Contains("x-amz-date"));
        Assert.True(request.Headers.Contains("User-Agent"));
        Assert.Equal("Data Retrieval/1.0 (Language=C#)", request.Headers.UserAgent.ToString());

        // Verify the 'host' header indirectly through the RequestUri property
        Assert.Contains("sellingpartnerapi-na.amazon.com", request.RequestUri.ToString());
    }
    
    private class MockAuthentication : IAuthentication
    {
        public Dictionary<string, string> AuthenticationHeaders() => new Dictionary<string, string>
        {
            // Example header; adjust as needed to fit the expected use case
            { "Authorization", "Bearer example-token" }
        };
    }
}