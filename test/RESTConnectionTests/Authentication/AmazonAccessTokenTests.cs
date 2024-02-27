using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using RESTConnection.Authentication;
using RESTConnection.Authentication.TokenRequest;
using Xunit;

namespace RESTConnectionTests.Authentication
{
    public class AmazonAccessTokenTests
    {
        [Fact]
        public void Constructor_InitializesPropertiesCorrectly()
        {
            // Arrange
            string clientId = "test-client-id";
            string clientSecret = "test-client-secret";
            string refreshToken = "test-refresh-token";
            Mock<ITokenRequestService> mockTokenRequestService = new Mock<ITokenRequestService>();

            // Mock the GetAccessToken to return a dummy token
            mockTokenRequestService.Setup(service => service.GetAccessToken(clientId, clientSecret, refreshToken))
                .ReturnsAsync("dummy-token");

            // Act
            AmazonAccessToken amazonAccessToken = new AmazonAccessToken(clientId, clientSecret, refreshToken, mockTokenRequestService.Object);

            // Assert
            Dictionary<string, string> headers = amazonAccessToken.AuthenticationHeaders();
            Assert.True(headers.ContainsKey("x-amz-access-token"));
            Assert.Equal("dummy-token", headers["x-amz-access-token"]);
        }

        [Fact]
        public void Dispose_DisposesTimerCorrectly()
        {
            // Arrange
            Mock<ITokenRequestService> mockTokenRequestService = new Mock<ITokenRequestService>();
            AmazonAccessToken amazonAccessToken = new AmazonAccessToken("client-id", "client-secret", "refresh-token", mockTokenRequestService.Object);

            // Act
            Exception recordedException = Record.Exception(() => amazonAccessToken.Dispose());

            // Assert
            Assert.Null(recordedException); // No exception should be thrown
        }
    }
}