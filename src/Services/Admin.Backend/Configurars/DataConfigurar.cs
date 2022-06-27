using Admin.Backend.Data;
using Admin.Backend.Services.Account;
using Admin.Backend.Services.Datasource;
using Admin.Backend.Services.Endpoint;

namespace Admin.Backend.Configurars
{
    public class DataConfigurar
    {
        public void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAccountStore>(sp =>
            {
                var cs = configuration["CONNECTION_STRING"] ?? throw new ArgumentNullException("CONNECTION_STRING");
                return new MySqlAccountStore(cs);
            });

            services.AddSingleton<IDatasourceStore>(sp =>
            {
                var cs = configuration["CONNECTION_STRING"] ?? throw new ArgumentNullException("CONNECTION_STRING");
                return new MySqlDatasourceStore(cs);
            });

            services.AddSingleton<IEndpointStore>(sp =>
            {
                var cs = configuration["CONNECTION_STRING"] ?? throw new ArgumentNullException("CONNECTION_STRING");
                return new MySqlEndpointStore(cs);
            });
        }
    }
}
