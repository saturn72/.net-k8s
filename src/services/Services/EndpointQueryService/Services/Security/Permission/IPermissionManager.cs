namespace EndpointQueryService.Services.Security.Permission
{
    public interface IPermissionManager
    {
        Task<bool> UserIsPermittedForEndpointAction(ActionContext context);
    }
}
