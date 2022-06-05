using System;

namespace Endpoints.Core.Events
{
    public class DomainEvent<TPayload>
    {
        public TPayload Payload { get; set; }
        public DateTime FiredOnUtc { get; set; }
    }
}
