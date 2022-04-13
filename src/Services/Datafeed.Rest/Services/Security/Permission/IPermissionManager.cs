using Datafeed.Rest.Domain;

namespace Datafeed.Rest.Services.Security.Permission
{
    public interface IPermissionManager
    {
        Task<bool> UserIsPermittedForEndpoint(string userId, AccountEndpoint endpoint);
    }
}
