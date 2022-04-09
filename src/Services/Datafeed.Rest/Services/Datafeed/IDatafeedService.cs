namespace Datafeed.Rest.Services.Datafeed
{
    public interface IDatafeedService
    {
        Task GetAll(GetAllContext context);
    }
}
