namespace RESTConnection.Authentication;

public interface IAuthentication
{
    Dictionary<string, string> AuthenticationHeaders();
}