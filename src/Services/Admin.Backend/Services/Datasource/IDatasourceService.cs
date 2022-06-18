using Admin.Backend.Domain;

namespace Admin.Backend.Services.Datasource
{
    public interface IDatasourceService
    {
        Task CreateDatasource(CreateContext<DatasourceDomainModel> context);
    }
}
