using Admin.Backend.Data;
using Admin.Backend.Services.Endpoint;

namespace Admin.Backend.Configurars
{
    public class DataConfigurar
    {
        public void Configure(IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<IEndpointStore>(sp => new MySqlEndpointStore(configuration.GetConnectionString("endpoint")));
        }
    }
}
