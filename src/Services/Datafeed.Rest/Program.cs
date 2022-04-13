using Datafeed.Rest.Configurars;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
_ = new ConfigurarRoot(builder);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
