using Datafeed.Rest.Domain;

namespace Datafeed.Rest.Services.Security.Permission
{
    public interface IPermissionManager
    {
        Task<bool> UserPermittedForEndpoint(string userId, Account account, string endpoint);
    }
}
