using LibroFacil.Application.Interfaces;
using LibroFacil.Application.Services;
using LibroFacil.Infrastructure.Persistence;
using LibroFacil.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ──────────────────────────────────────────────────────────────────
// 1. DbContext — EF Core con SQL Server
// ──────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<LibroFacilDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ──────────────────────────────────────────────────────────────────
// 2. DIP en acción: la interfaz se resuelve con la implementación concreta
//    LibroService solo conoce ILibroRepository, nunca LibroRepositoryEf
// ──────────────────────────────────────────────────────────────────
builder.Services.AddScoped<ILibroRepository, LibroRepositoryEf>();
builder.Services.AddScoped<LibroService>();

// ──────────────────────────────────────────────────────────────────
// 3. Controllers + JSON (serialización camelCase estándar)
// ──────────────────────────────────────────────────────────────────
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // mantener PascalCase
    });

var app = builder.Build();

// ──────────────────────────────────────────────────────────────────
// 4. Middleware
// ──────────────────────────────────────────────────────────────────
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
