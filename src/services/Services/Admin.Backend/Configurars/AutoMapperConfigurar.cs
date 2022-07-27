using Admin.Backend.Domain;
using Admin.Backend.Models;
using AutoMapper;

namespace Admin.Backend.Configurars
{
    public class AutoMapperConfigurar : Profile
    {
        public AutoMapperConfigurar()
        {
            CreateMap<CreateEndpointApiModel, EndpointDomainModel>();
        }

        public void Configure(IServiceCollection services)
        {
            services.AddAutoMapper(this.GetType());
        }
    }
}
