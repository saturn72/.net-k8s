using System.Threading.Tasks;

namespace Endpoints.Core.Events
{
    public class EventPublisher : IEventPublisher
    {
        public EventPublisher(ISystemClock clock)
        {
            _clock = clock;
        }
        public Task Publish<TPayload>(DomainEvent<TPayload> @event)
        {
            @event.FiredOnUtc = _systemClock.
            return Task.CompletedTask;
        }
    }
}
