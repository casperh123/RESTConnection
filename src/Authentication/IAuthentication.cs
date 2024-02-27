namespace DataDownloader.Connection.Authentication;

public interface IAuthentication
{
    string ClientId { get; }
    string? Scope { get; }
    
    string HeaderFormat();
}