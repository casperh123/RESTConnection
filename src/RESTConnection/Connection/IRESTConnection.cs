namespace RESTConnection.Connection;

public interface IRESTConnection
{
    int InitialRetryDelay { get; set; }
    public Task<HttpResponseMessage> RequestAsync(HttpMethod method, string endpoint, object content = null ,params Parameter[] parameters);
}