
namespace Admin.Backend.Configurars
{
    public class CorsConfigurar
    {
        internal const string PolicyName = "default-cors-policy";
        public void Configure(IServiceCollection services, IConfiguration configuration)
        {
            var cors = configuration.GetSection("CORS")
                .GetChildren()
                .Select(c => c.Value)
                .ToArray();

            //.AsEnumerable()
            //.ToArray();

            if (cors.IsNullOrEmpty())
                throw new ArgumentNullException("CORS");
            services.AddCors(options =>
            {

                options.AddPolicy(PolicyName,
                             policy =>
                             {
                                 policy.WithOrigins(cors)
                                                     .AllowAnyHeader()
                                                     .AllowAnyMethod();
                             });
            });
        }
    }
}
