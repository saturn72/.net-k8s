using Admin.Backend.Configurars;

namespace Admin.Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            new ConfigurarRoot().Configure(builder);

            var app = builder.Build();
            app.UseCors(CorsConfigurar.PolicyName);
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}