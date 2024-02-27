namespace RESTConnection.Authentication;

public class EconomicAuthentication : AbstractAuthentication
{
    private Dictionary<string, string> _authenticationHeaders;

    public EconomicAuthentication(string appId, string agreementToken)
    {
        _authenticationHeaders = new Dictionary<string, string>()
        {
            { "X-AppSecretToken", $"{appId}" },
            { "X-AgreementGrantToken", $"{agreementToken}" }
        };
    }

    public override Dictionary<string, string> AuthenticationHeaders()
    {
        return _authenticationHeaders;
    }
}