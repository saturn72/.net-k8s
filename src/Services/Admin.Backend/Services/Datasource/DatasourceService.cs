using Admin.Backend.Domain;
using AnyService.Events;

namespace Admin.Backend.Services.Datasource
{
    public class DatasourceService : IDatasourceService
    {
        private readonly IValidator<CreateContext<DatasourceDomainModel>> _createValidator;
        private readonly IValidator<ReadByIdContext<DatasourceDomainModel>> _readByIdValidator;
        private readonly IDatasourceStore _store;
        private readonly IDomainEventBus _events;
        private readonly ILogger<IDatasourceService> _logger;

        public DatasourceService(
            IValidator<CreateContext<DatasourceDomainModel>> createValidator,
            IValidator<ReadByIdContext<DatasourceDomainModel>> readByIdValidator,
            IDatasourceStore store,
            IDomainEventBus events,
            ILogger<IDatasourceService> logger)
        {
            _createValidator = createValidator;
            _readByIdValidator = readByIdValidator;
            _store = store;
            _events = events;
            _logger = logger;
        }
        public async Task CreateDatasource(CreateContext<DatasourceDomainModel> context)
        {
            if (!await _createValidator.IsValidFor(context) || context.IsError)
            {
                context?.SetErrors(
                    $"{nameof(context)}: is null, or have empty/null value for {nameof(context.ToCreate.Account)} and/or {nameof(context.ToCreate.Name)} and/or {nameof(context.ToCreate.Type)}",
                    "Bad or missing data");
                _logger.LogInformation(context.Error);
                return;
            }

            context.Created = await _store.Create(context.ToCreate);

            if (context.Created == default)
            {
                context.SetErrors(
                    $"Failed to create datasource for: {context.ToCreate.ToJsonString()}",
                    "Failed to create new datasource record");
                _logger.LogError(context.Error);
                return;
            }

            _ = _events.Publish(EventKeys.Datasource.Created, context);
        }

        public async Task GetDatasourceById(ReadByIdContext<DatasourceDomainModel> context)
        {
            if (!await _readByIdValidator.IsValidFor(context) || context.IsError)
            {
                _logger.LogInformation(context.Error);
                return;
            }

            context.Read= await _store.GetDatasourceById(context.Id);

            if (context.Read == default)
            {
                context.SetErrors(
                    $"Failed to read datasource with \'id\' = {context.Id}",
                    "Failed to read datasource");
                _logger.LogError(context.Error);
                return;
            }
        }
    }
}
