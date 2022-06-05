using Microsoft.AspNetCore.Authentication;

namespace Endpoints.Common.Events
{
    public class EventPublisher : IEventPublisher
    {
        private readonly ISystemClock _clock;

        public EventPublisher(ISystemClock clock)
        {
            _clock = clock;
        }
        public Task Publish<TPayload>(DomainEvent<TPayload> @event)
        {
            @event.FiredOnUtc = _clock.UtcNow.UtcDateTime;
            return Task.CompletedTask;
        }
    }
}
