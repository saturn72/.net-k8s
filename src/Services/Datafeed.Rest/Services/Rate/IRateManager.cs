using Datafeed.Rest.Domain;

namespace Datafeed.Rest.Services.Rate
{
    public interface IRateManager
    {
        Task<bool> UserExceededAccessToAccountEndpointVersionAction(string userId, AccountEndpoint endpoint, string version, string action);
    }
}
