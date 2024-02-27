using RESTConnection.Authentication;
using System.Collections.Generic;
using Xunit;

namespace RESTConnectionTests.Authentication
{
    public class EconomicAuthenticationTests
    {
        [Fact]
        public void Constructor_SetsAuthenticationHeadersCorrectly()
        {
            string appId = "test-app-id";
            string agreementToken = "test-agreement-token";

            EconomicAuthentication economicAuthentication = new EconomicAuthentication(appId, agreementToken);
            Dictionary<string, string> headers = economicAuthentication.AuthenticationHeaders();

            Assert.NotNull(headers);
            Assert.Equal(2, headers.Count);
            Assert.True(headers.ContainsKey("X-AppSecretToken"));
            Assert.True(headers.ContainsKey("X-AgreementGrantToken"));
            Assert.Equal(appId, headers["X-AppSecretToken"]);
            Assert.Equal(agreementToken, headers["X-AgreementGrantToken"]);
        }
    }
}