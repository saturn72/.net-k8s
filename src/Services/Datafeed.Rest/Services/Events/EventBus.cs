namespace Datafeed.Rest.Services.Events
{
    public class EventBus : IEventBus
    {
        public Task Publish<TEvent>(TEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
