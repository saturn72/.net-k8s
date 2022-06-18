using Admin.Backend.Domain;

namespace Admin.Backend.Services.Security
{
    public class PermissionManager : IPermissionManager
    {
        public Task<bool> UserIsPermittedForAction(ContextBase context)
        {
            return Task.FromResult(true);
        }
    }
}
