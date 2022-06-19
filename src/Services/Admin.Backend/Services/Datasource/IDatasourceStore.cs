using Admin.Backend.Domain;

namespace Admin.Backend.Services.Datasource
{
    public interface IDatasourceStore
    {
        Task Create(CreateContext<DatasourceDomainModel> context);
        Task<DatasourceDomainModel> GetByName(string name);
    }
}
