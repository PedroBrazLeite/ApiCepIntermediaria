using MinhaApi.Clients;
using Refit;
using MinhaApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddRefitClient<ICep>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://opencep.com"));


builder.Services.AddScoped<ICepService, CepService>();

var app = builder.Build();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();