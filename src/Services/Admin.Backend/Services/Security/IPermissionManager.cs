using Admin.Backend.Domain;

namespace Admin.Backend.Services.Security
{
    public interface IPermissionManager
    {
        Task<bool> UserIsPermittedForEndpointAction(ContextBase context);
    }
}
