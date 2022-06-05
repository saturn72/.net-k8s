using Admin.Backend.Domain;

namespace Admin.Backend.Services.Endpoint
{
    public interface IEndpointService
    {
        Task CreateEndpoint(CreateContext<EndpointDomainModel> context);
    }
}
