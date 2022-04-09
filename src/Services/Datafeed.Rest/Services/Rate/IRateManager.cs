using Datafeed.Rest.Domain;

namespace Datafeed.Rest.Services.Rate
{
    public interface IRateManager
    {
        Task<bool> UserExceededAccessToEndpointAction(string userId, Account account, string endpoint, string action);
    }
}
