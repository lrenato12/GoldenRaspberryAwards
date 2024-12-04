using API.Models;
using API.Utils;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configuração do MovieContext com banco de dados SQLite
builder.Services.AddDbContext<MovieContext>(options =>
    options.UseSqlite("DataSource=movies.db"));

// Registra o serviço do MovieService
builder.Services.AddScoped<MovieContext>();

var app = builder.Build();

// Executa o carregamento do CSV ao iniciar a aplicação
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MovieContext>();

    // Cria o banco de dados se não existir
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
