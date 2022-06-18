using Admin.Backend.Domain;
using AnyService.Events;

namespace Admin.Backend.Services.Datasource
{
    public class DatasourceService : IDatasourceService
    {
        private readonly IValidator<CreateContext<DatasourceDomainModel>> _validator;
        private readonly IDatasourceStore _Datasources;
        private readonly IDomainEventBus _events;
        private readonly ILogger<IDatasourceService> _logger;

        public DatasourceService(
            IValidator<CreateContext<DatasourceDomainModel>> validator,
            IDatasourceStore Datasources,
            IDomainEventBus events,
            ILogger<IDatasourceService> logger)
        {
            _validator = validator;
            _Datasources = Datasources;
            _events = events;
            _logger = logger;
        }
        public async Task CreateDatasource(CreateContext<DatasourceDomainModel> context)
        {
            if (!await _validator.IsValidFor(context) || context.IsError)
            {
                _logger.LogInformation(context.Error);
                return;
            }
            await _Datasources.Create(context);
            if (context.IsError)
            {
                _logger.LogError(context.Error);
                return;
            }
            _ = _events.Publish(EventKeys.Datasource.Created, context);
        }
    }
}
