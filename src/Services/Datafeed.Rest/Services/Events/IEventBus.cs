namespace Datafeed.Rest.Services.Events
{
    public interface IEventBus
    {
        Task Publish<TEvent>(TEvent @event);
    }
}
