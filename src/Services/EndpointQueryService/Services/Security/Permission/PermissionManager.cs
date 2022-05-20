namespace EndpointQueryService.Services.Security.Permission
{
    public class PermissionManager : IPermissionManager
    {
        public Task<bool> UserIsPermittedForEndpointAction(ActionContext context)
        {
            return Task.FromResult(true);
        }
    }
}
