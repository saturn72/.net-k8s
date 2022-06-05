using Admin.Backend.Domain;

namespace Admin.Backend.Services.Security
{
    public class PermissionManager : IPermissionManager
    {
        public Task<bool> UserIsPermittedForEndpointAction(ContextBase context)
        {
            return Task.FromResult(true);
        }
    }
}
