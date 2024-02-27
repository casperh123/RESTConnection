namespace DataDownloader.Connection.Authentication;

public class AmazonAdsAccessToken : IAuthentication
{
    public string ClientId { get; }
    public string? Scope { get; }
    private string _clientSecret;
    private string _accessToken;
    
    public AmazonAdsAccessToken(string clientId, string clientSecret)
    {
        ClientId = clientId;
        _clientSecret = clientSecret;
        
    }
    
    public string HeaderFormat()
    {
        return _accessToken;
    }
}