namespace EndpointQueryService.Services.Events
{
    public interface IEventBus
    {
        Task Publish<TEvent>(TEvent @event);
    }
}
