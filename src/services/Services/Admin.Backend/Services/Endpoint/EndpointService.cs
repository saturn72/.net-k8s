using Admin.Backend.Domain;
using AnyService.Events;

namespace Admin.Backend.Services.Endpoint
{
    public class EndpointService : IEndpointService
    {
        private readonly IValidator<CreateContext<EndpointDomainModel>> _validator;
        private readonly IEndpointStore _endpoints;
        private readonly IDomainEventBus _events;
        private readonly ILogger<IEndpointService> _logger;

        public EndpointService(
            IValidator<CreateContext<EndpointDomainModel>> validator,
            IEndpointStore endpoints,
            IDomainEventBus events,
            ILogger<IEndpointService> logger)
        {
            _validator = validator;
            _endpoints = endpoints;
            _events = events;
            _logger = logger;
        }
        public async Task CreateEndpoint(CreateContext<EndpointDomainModel> context)
        {
            if (!await _validator.IsValidFor(context) || context.IsError)
            {
                _logger.LogInformation(context.Error);
                return;
            }
            await _endpoints.Create(context.ToCreate);

            if (context.IsError)
            {
                _logger.LogError(context.Error);
                return;
            }
            _ = _events.Publish(EventKeys.Endpoint.Created, context);
        }
    }
}
