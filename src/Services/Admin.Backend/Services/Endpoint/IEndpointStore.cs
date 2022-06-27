using Admin.Backend.Domain;

namespace Admin.Backend.Services.Endpoint
{
    public interface IEndpointStore
    {
        Task<EndpointDomainModel> Create(EndpointDomainModel endpoint);
        Task<EndpointDomainModel> GetByPath(string path);
    }
}
