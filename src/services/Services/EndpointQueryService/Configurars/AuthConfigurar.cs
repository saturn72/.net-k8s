using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace EndpointQueryService.Configurars
{
    public class AuthConfigurar
    {
        public void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
                {
                    o.Authority = configuration["IdentityUrl"];
                });
        }
    }
}
