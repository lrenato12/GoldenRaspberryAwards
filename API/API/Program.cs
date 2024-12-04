using API.Models;
using API.Service;
using API.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

IWebHostEnvironment env = builder.Environment;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configura��o do MovieContext com banco de dados SQLite
builder.Services.AddDbContext<MovieContext>(options =>
    options.UseSqlite("DataSource=movies.db"));

builder.Services.AddScoped<MovieContext>();
builder.Services.AddScoped<ProducerIntervalService>();

// Adiciona a configura��o do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Movie API",
        Version = "v1",
        Description = "API para gerenciamento da lista de indicados e vencedores da categoria Pior Filme do Golden Raspberry Awards."
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie API v1");
        c.RoutePrefix = string.Empty;
    });
}

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
//https://localhost:7038/index.html
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
