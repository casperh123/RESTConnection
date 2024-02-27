namespace DataDownloader.Connection.HttpClientSingleton;

public static class HttpClientSingleton
{
    // Static readonly field ensures a single instance is created and lazily initialized.
    private static readonly HttpClient _instance = new HttpClient();

    // Public static property to provide access to the instance.
    public static HttpClient Instance => _instance;

    // Static constructor to enforce non-public creation
    static HttpClientSingleton()
    {
    }
}