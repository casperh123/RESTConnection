using RESTConnection.Connection;
using RESTConnection.Connection.RequestBuilder.Url;
using Xunit;

namespace RESTConnectionTests.Connection.RequestBuilder.Url
{
    public class RequestUrlTests
    {
        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenUrlIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RequestUrl(null));
        }

        [Fact]
        public void GetRequestUrl_ReturnsCorrectUrl_WhenCalledWithEndpoint()
        {
            RequestUrl requestUrl = new RequestUrl("www.example.com");
            string result = requestUrl.GetRequestUrl("testEndpoint");

            Assert.Equal("https://www.example.com/testEndpoint", result);
        }

        [Fact]
        public void GetRequestUrl_ThrowsArgumentNullException_WhenEndpointIsNull()
        {
            RequestUrl requestUrl = new RequestUrl("www.example.com");
            Assert.Throws<ArgumentNullException>(() => requestUrl.GetRequestUrl(null));
        }

        [Fact]
        public void GetRequestUrl_ReturnsUrlWithQueryParameters_WhenCalledWithParameters()
        {
            RequestUrl requestUrl = new RequestUrl("www.example.com");
            string result = requestUrl.GetRequestUrl("test", new Parameter("key", "value"), new Parameter("anotherKey", "anotherValue"));

            Assert.Contains("?key=value&anotherKey=anotherValue", result);
        }

        [Fact]
        public void GetRequestUrl_GroupsParametersWithSameKey_AndCommaSeparatesValues()
        {
            RequestUrl requestUrl = new RequestUrl("www.example.com");
    
            string result = requestUrl.GetRequestUrl("test", new Parameter("key", "value1"), new Parameter("key", "value2"));
    
            Assert.Contains("?key=value1,value2", result);
            
            Assert.Equal("https://www.example.com/test?key=value1,value2", result);
        }
        
        [Fact]
        public void GetRequestUrl_EncodesParametersInQueryString()
        {
            RequestUrl requestUrl = new RequestUrl("www.example.com");
            string result = requestUrl.GetRequestUrl("test", new Parameter("key with spaces", "value/with?special=characters&"));

            Assert.Contains("?key+with+spaces=value%2Fwith%3Fspecial%3Dcharacters%26", result);
        }
    }
}