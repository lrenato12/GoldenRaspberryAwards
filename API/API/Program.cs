using API.Models;
using API.Service;
using API.Service.CSVLoader;
using API.Service.MovieDataImporter;
using API.Service.MovieService;
using API.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

IWebHostEnvironment env = builder.Environment;

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configuração do MovieContext com banco de dados SQLite
builder.Services.AddDbContext<MovieContext>(options => options.UseSqlite("DataSource=movies.db"));

#region [ ADD SCOPED ]
builder.Services.AddScoped<MovieContext>();
builder.Services.AddScoped<ProducerIntervalService>();
builder.Services.AddScoped<ICsvLoaderService, CsvLoaderService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<MovieDataImporter>(); 
#endregion

// Adiciona a configuração do Swagger
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

// Executa o carregamento do CSV ao iniciar a aplicação
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MovieContext>();

    // Cria o banco de dados se não existir
    dbContext.Database.EnsureCreated();

    // Caminho do arquivo CSV
    var csvPath = Path.Combine(AppContext.BaseDirectory, "movies.csv");

    // Carrega os dados do CSV
    using var scopeMovieData = app.Services.CreateScope();
    var importer = scopeMovieData.ServiceProvider.GetRequiredService<MovieDataImporter>();
    importer.Import(csvPath);
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