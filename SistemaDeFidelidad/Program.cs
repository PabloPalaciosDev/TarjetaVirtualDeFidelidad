using Microsoft.EntityFrameworkCore;
using SistemaDeFidelidad.DbContext;
using SistemaDeFidelidad.Interfaces;
using SistemaDeFidelidad.Models;
using SistemaDeFidelidad.Repository;
using SistemaDeFidelidad.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//JWT config
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:8081") // Cambia por la URL del cliente
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


//database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(10, 5, 8))));

builder.Services.AddAuthorization();

//inyección de dependencias
//Servicios
builder.Services.AddScoped<ServiceClienteParticipante>();
builder.Services.AddScoped<ServiceTarjetaFidelidad>();
//Repositorios
builder.Services.AddScoped<IRepository<ClienteParticipante>, Repository<ClienteParticipante>>();
builder.Services.AddScoped<IRepository<TarjetaFidelidad>, Repository<TarjetaFidelidad>>();
builder.Services.AddScoped<IRepository<DescuentosCliente>, Repository<DescuentosCliente>>();

//swagger

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1.0",
        Title = "Sistema de Fidelidad API",
        Description = "API para el manejo de clientes y tarjetas de fidelidad"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese 'Bearer [token]' en el campo de autorización."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
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

app.UseCors("AllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
