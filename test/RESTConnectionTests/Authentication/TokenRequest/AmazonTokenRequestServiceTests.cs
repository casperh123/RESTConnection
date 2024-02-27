using System.Net;
using System.Text.Json;
using Moq;
using Moq.Protected;
using RESTConnection.Authentication.TokenRequest;
using Xunit;

namespace RESTConnectionTests.Authentication.TokenRequest
{
    public class AmazonTokenRequestServiceTests
    {
        [Fact]
        public async Task GetAmazonAccessToken_ReturnsTokenOnSuccess()
        {
            // Arrange
            string expectedAccessToken = "test-access-token";
            Mock<HttpMessageHandler> mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(new { access_token = expectedAccessToken }))
                });

            HttpClient client = new HttpClient(mockHandler.Object);
            ITokenRequestService service = new AmazonTokenRequestService(client);

            // Act
            string token = await service.GetAccessToken("client-id", "client-secret", "refresh-token");

            // Assert
            Assert.Equal(expectedAccessToken, token);
        }

        [Fact]
        public async Task GetAmazonAccessToken_ThrowsWhenAccessTokenNotFound()
        {
            // Arrange
            Mock<HttpMessageHandler> mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{}")
                });

            HttpClient client = new HttpClient(mockHandler.Object);
            ITokenRequestService service = new AmazonTokenRequestService(client);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetAccessToken("client-id", "client-secret", "refresh-token"));
        }
    }
}
