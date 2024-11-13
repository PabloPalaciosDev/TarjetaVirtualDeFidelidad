using Microsoft.EntityFrameworkCore;
using SistemaDeFidelidad.DbContext;
using SistemaDeFidelidad.Interfaces;
using SistemaDeFidelidad.Models;
using SistemaDeFidelidad.Repository;
using SistemaDeFidelidad.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


//database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthorization();

//inyección de dependencias
//builder.Services.AddScoped<ClienteParticipantesController>();
builder.Services.AddScoped<ServiceClienteParticipante>();
builder.Services.AddScoped<ServiceTarjetaFidelidad>();
builder.Services.AddScoped<IRepository<ClienteParticipante>, Repository<ClienteParticipante>>();
builder.Services.AddScoped<IRepository<TarjetaFidelidad>, Repository<TarjetaFidelidad>>();

//swagger

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1.0",
        Title = "Sistema de Fidelidad API",
        Description = "API para el manejo de clientes y tarjetas de fidelidad"
    });
});


builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true; // Incluye información sobre las versiones en las respuestas
    options.AssumeDefaultVersionWhenUnspecified = true; // Usa la versión predeterminada si no se especifica
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0); // Versión predeterminada
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManager API V1");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
