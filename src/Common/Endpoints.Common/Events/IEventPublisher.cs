using System.Threading.Tasks;

namespace Endpoints.Common.Events
{
    public interface IEventPublisher
    {
        Task Publish<TPayload>(DomainEvent<TPayload> @event);
    }
}
