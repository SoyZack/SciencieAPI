using Microsoft.EntityFrameworkCore;
using SciencieAPI.Mappings;
using SciencieAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Registra los controladores HTTP de la API.
builder.Services.AddControllers();

// Registra el DbContext y lo configura para usar SQL Server
builder.Services.AddDbContext<DescubrimientosContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registra el perfil MappingProfile para convertir entidades a DTOs
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

// Genera la documentación OpenAPI y la interfaz /swagger
// para probar los endpoints desde el navegador.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// OpenAPI nativo de ASP.NET Core: mantiene disponible /openapi/v1.json.
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseCors("PermitirTodo");

// En desarrollo se habilita la documentación de la API.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI( c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CientificosAPI V1");
        c.RoutePrefix = string.Empty;
    });

}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
