namespace Datafeed.Rest.Services.Security.Permission
{
    public class PermissionManager : IPermissionManager
    {
        public Task<bool> UserIsPermittedForTemplateAction(string userId, ActionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
