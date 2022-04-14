namespace Datafeed.Rest.Services.Rate
{
    public interface IRateManager
    {
        Task<bool> UserExceededAccessToAccountEndpointVersionAction(ActionContext context);
        Task IncrementAccessToAccountEndpointVersionAction(ActionContext context);
    }
}
