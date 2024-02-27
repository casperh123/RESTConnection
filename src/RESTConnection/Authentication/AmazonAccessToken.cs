using RESTConnection.Authentication.TokenRequest;
using RESTConnection.Connection;

namespace RESTConnection.Authentication
{
    public class AmazonAccessToken : IAuthentication, IDisposable
    {
        private string _clientId;
        private string _accessToken;
        private readonly string _clientSecret;
        private readonly string _refreshToken;
        private Timer _refreshTimer;
        private const int RefreshIntervalMinutes = 59;

        public AmazonAccessToken(string clientId, string clientSecret, string refreshToken)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _refreshToken = refreshToken;

            // Call RefreshAccessToken immediately and set the timer for subsequent refreshes
            RefreshAccessToken().Wait();
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            var interval = TimeSpan.FromMinutes(RefreshIntervalMinutes).TotalMilliseconds;
            _refreshTimer = new Timer(async _ => await RefreshAccessToken(), 
                null, 
                TimeSpan.FromMilliseconds(interval), 
                TimeSpan.FromMilliseconds(interval));
        }
        
        public Dictionary<string, string> AuthenticationHeaders()
        {
            return new Dictionary<string, string>()
            {
                { "x-amz-access-token", $"{_accessToken}" }
            };
        }
        
        private async Task RefreshAccessToken()
        {
            _accessToken = await TokenRequests.GetAmazonAccessToken(_clientId, _clientSecret, _refreshToken);
        }

        public void Dispose()
        {
            _refreshTimer?.Dispose();
        }
    }
}