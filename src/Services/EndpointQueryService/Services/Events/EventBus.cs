namespace EndpointQueryService.Services.Events
{
    public class EventBus : IEventBus
    {
        public Task Publish<TEvent>(TEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}
