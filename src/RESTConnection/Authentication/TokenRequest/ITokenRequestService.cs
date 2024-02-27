using System.Threading.Tasks;

namespace RESTConnection.Authentication.TokenRequest
{
    public interface ITokenRequestService
    {
        Task<string> GetAccessToken(string clientId, string clientSecret, string refreshToken);
    }
}