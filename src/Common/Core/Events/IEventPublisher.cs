using System.Threading.Tasks;

namespace Endpoints.Core.Events
{
    public interface IEventPublisher
    {
        Task Publish<TPayload>(DomainEvent<TPayload> @event);
    }
}
