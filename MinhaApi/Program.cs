using Microsoft.Extensions.Caching.Hybrid;
using MinhaApi.Clients;
using Refit;
using MinhaApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddHybridCache(options =>
{
    options.MaximumPayloadBytes = 1024 * 1024 * 10; 
    options.MaximumKeyLength = 512;

    options.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        Expiration = TimeSpan.FromMinutes(30),
        LocalCacheExpiration = TimeSpan.FromMinutes(30)
    };
});

builder.Services.AddRefitClient<ICep>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://opencep.com"));

builder.Services.AddScoped<ICepService, CepService>();

var app = builder.Build();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();