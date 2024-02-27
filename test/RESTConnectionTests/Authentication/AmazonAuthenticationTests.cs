using Moq;
using RESTConnection.Authentication;
using RESTConnection.Authentication.TokenRequest;
using Xunit;

namespace RESTConnectionTests.Authentication;

public class AmazonAuthenticationTests
{
    [Fact]
    public async Task AuthenticationHeaders_LazilyLoadsTokenWithoutBlocking()
    {
        // Arrange
        var clientId = "test-client-id";
        var clientSecret = "test-client-secret";
        var refreshToken = "test-refresh-token";
        var expectedToken = "dummy-token";
        var mockTokenRequestService = new Mock<ITokenRequestService>();
        mockTokenRequestService.Setup(service => service.GetAccessToken(clientId, clientSecret, refreshToken))
            .ReturnsAsync(expectedToken);

        var amazonAuthentication = new AmazonAuthentication(clientId, clientSecret, refreshToken, mockTokenRequestService.Object);

        // Act
        var initialTime = DateTime.Now;
        var headers = amazonAuthentication.AuthenticationHeaders();
        var elapsed = DateTime.Now - initialTime;

        // Assert
        Assert.Contains("x-amz-access-token", headers.Keys);
        Assert.Equal(expectedToken, headers["x-amz-access-token"]);
        // Ensure the call was quick, implying no blocking occurred. Adjust the threshold as needed.
        Assert.True(elapsed < TimeSpan.FromSeconds(1), "Token retrieval should be fast and non-blocking.");

        // Additionally, verify that the token service was called, implying lazy loading.
        mockTokenRequestService.Verify(service => service.GetAccessToken(clientId, clientSecret, refreshToken), Times.Once);
    }
}