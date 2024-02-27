using DataDownloader.Connection.RESTConnection;

namespace DataDownloader.Connection.Authentication
{
    public class AmazonAccessToken : IAuthentication, IDisposable
    {
        public string ClientId { get; }
        public string? Scope { get; set; }
        private string _accessToken;
        private readonly string _clientSecret;
        private readonly string _refreshToken;
        private Timer _refreshTimer;
        private const int RefreshIntervalMinutes = 59;

        public AmazonAccessToken(string clientId, string clientSecret, string refreshToken)
        {
            ClientId = clientId;
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

        public string HeaderFormat()
        {
            return _accessToken;
        }

        private async Task RefreshAccessToken()
        {
            _accessToken = await TokenRequests.GetAmazonAccessGetAccessToken(ClientId, _clientSecret, _refreshToken);
        }

        public void Dispose()
        {
            _refreshTimer?.Dispose();
        }
    }
}