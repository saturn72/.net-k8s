using EndpointQueryService.Configurars;

var builder = WebApplication.CreateBuilder(args);
var c = new PrimaryConfigurar(builder);
c.Configure();

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
