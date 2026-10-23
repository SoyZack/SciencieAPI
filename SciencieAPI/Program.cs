using Microsoft.EntityFrameworkCore;
using SciencieAPI.Data;
using SciencieAPI.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Registra los controladores HTTP de la API.
builder.Services.AddControllers();

// EF Core: registra el DbContext y lo configura para usar SQL Server
// con la cadena de conexión "DefaultConnection" definida en appsettings.json.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper: registra el perfil MappingProfile para convertir entidades a DTOs
// y DTOs a entidades. Se usa la extensión AddAutoMapper incluida en AutoMapper.
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

// Swagger (Swashbuckle): genera la documentación OpenAPI y la interfaz /swagger
// para probar los endpoints desde el navegador.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// OpenAPI nativo de ASP.NET Core: mantiene disponible /openapi/v1.json.
builder.Services.AddOpenApi();

var app = builder.Build();

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

// Expone los controladores registrados con AddControllers.
app.MapControllers();

app.Run();
