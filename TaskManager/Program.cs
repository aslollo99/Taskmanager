using Microsoft.EntityFrameworkCore;
using TaskManager.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Connessione al DB (usiamo Docker SQL Server)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Questo abilita il servizio di file statici
app.UseStaticFiles();

// Questo reindirizza la richiesta di base ("/") a index.html se non ci sono altri percorsi
app.MapFallbackToFile("index.html");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();