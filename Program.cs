using System.Net.Http.Headers;
using DotNetEnv;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// Register the repository so we can inject it into the controller
builder.Services.AddScoped<backend_II.Database.ProductRepository>();

// HttpClient con nombre para PokeAPI, administrado por IHttpClientFactory.
// La fábrica reutiliza conexiones y evita el agotamiento de sockets de hacer "new HttpClient()".
builder.Services.AddHttpClient(backend_II.Services.PokeApiService.ClientName, client =>
{
    client.BaseAddress = new Uri(builder.Configuration["PokeApi:BaseUrl"] ?? "https://pokeapi.co/api/v2/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    client.Timeout = TimeSpan.FromSeconds(10);
});

// Service that consumes PokeAPI through the named HttpClient
builder.Services.AddScoped<backend_II.Services.PokeApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
