using RESTConnection.Authentication.TokenRequest;
using RESTConnection.Connection;

namespace RESTConnection.Authentication
{
    public class AmazonAuthentication : IAuthentication, IDisposable
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _refreshToken;
        private string _accessToken;
        private Timer _refreshTimer;
        private const int RefreshIntervalMinutes = 59;
        private readonly ITokenRequestService _tokenRequestService;

        public AmazonAuthentication(string clientId, string clientSecret, string refreshToken, ITokenRequestService tokenRequestService)
        {
            _clientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
            _clientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));
            _refreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));
            _tokenRequestService = tokenRequestService ?? throw new ArgumentNullException(nameof(tokenRequestService));

            RefreshAccessToken();
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
            if (_accessToken == null)
            {
                RefreshAccessToken().Wait();
            }
            
            return new Dictionary<string, string>()
            {
                { "x-amz-access-token", $"{_accessToken}" }
            };
        }
        
        private async Task RefreshAccessToken()
        {
            _accessToken = await _tokenRequestService.GetAccessToken(_clientId, _clientSecret, _refreshToken);
        }

        public void Dispose()
        {
            _refreshTimer?.Dispose();
        }
    }
}