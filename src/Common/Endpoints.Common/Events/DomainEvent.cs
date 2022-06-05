
namespace Endpoints.Common.Events
{
    public record DomainEvent<TPayload>
    {
        public TPayload Payload { get; set; }
        public DateTime FiredOnUtc { get; set; }
    }
}
