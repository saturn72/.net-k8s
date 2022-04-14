namespace Datafeed.Rest.Services.Security.Permission
{
    public interface IPermissionManager
    {
        Task<bool> UserIsPermittedForTemplateAction(string userId, ActionContext context);
    }
}
