using API.Models;
using API.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Executa o carregamento do CSV ao iniciar a aplica��o
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MovieContext>();

    // Cria o banco de dados se n�o existir
    dbContext.Database.EnsureCreated();

    // Caminho do arquivo CSV
    var csvPath = Path.Combine(AppContext.BaseDirectory, "movies.csv");

    // Carrega os dados do CSV
    CsvLoader.LoadCsvData(dbContext, csvPath);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
