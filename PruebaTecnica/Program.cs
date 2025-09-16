using Microsoft.EntityFrameworkCore;
using PruebaTecnica.BD;

var builder = WebApplication.CreateBuilder(args);

// Registrar CoreBD con cadena de conexión de appsettings.json
builder.Services.AddDbContext<CoreBD>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

// Swagger (opcional)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
