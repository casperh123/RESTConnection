namespace DataDownloader.Connection.Authentication;

public class EconomicAuthentication : IAuthentication
{
    public string ClientId { get; }
    public string? Scope { get; }
    private string _agreementToken;

    public EconomicAuthentication(string appId, string agreementToken)
    {
        ClientId = appId;
        _agreementToken = agreementToken;
        Scope = null;
    }
    
    public string HeaderFormat()
    {
        return _agreementToken;
    }
}