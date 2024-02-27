using System.Net.Http;

namespace RESTConnection
{
    public static class HttpClientSingleton
    {
        private static HttpClient _instance = new HttpClient();

        // Static constructor to enforce non-public creation
        static HttpClientSingleton()
        {
        }

        // Public static property to provide access to the instance.
        public static HttpClient Instance => _instance;

        // Method to inject a mock HttpClient instance for testing
        public static void InjectMockClient(HttpClient mockClient)
        {
            _instance = mockClient;
        }
    }
}