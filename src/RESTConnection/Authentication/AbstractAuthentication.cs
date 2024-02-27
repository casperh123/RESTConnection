namespace RESTConnection.Authentication;

public abstract class AbstractAuthentication : IAuthentication
{
    public abstract Dictionary<string, string> AuthenticationHeaders();
}