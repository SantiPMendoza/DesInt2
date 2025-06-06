using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AtecaAPI.Models.Entity;
using AtecaAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using AtecaAPI.Repository.IRepository;
using AtecaAPI.Repository;
using AtecaAPI.AutoMapper;
using Microsoft.AspNetCore.Authorization;
using AtecaAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Agrega el DbContext con cadena de conexión
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

// Configura Identity con tu AppUser personalizado
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Registrar IMemoryCache
builder.Services.AddMemoryCache();


// Add services to the container.
builder.Services.AddScoped<IAdministradorRepository, AdministradorRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProfesorRepository, ProfesorRepository>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<IGrupoClaseRepository, GrupoClaseRepository>();
builder.Services.AddScoped<IFranjaHorariaRepository, FranjaHorariaRepository>();
builder.Services.AddScoped<IDiaNoLectivoRepository, DiaNoLectivoRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

// Configurar autenticación con JWT y Google
var key = builder.Configuration.GetValue<string>("ApiSettings:SecretKey");
var googleClientId = builder.Configuration.GetValue<string>("Google:ClientId"); // Configura en appsettings.json

builder.Services.AddAuthentication()
    // JWT con clave propia (WPF, login tradicional)
    .AddJwtBearer("JwtOwn", options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    })
    // JWT Google (Angular)
    .AddJwtBearer("JwtGoogle", options =>
    {
        options.Authority = "https://accounts.google.com";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "https://accounts.google.com",
            ValidateAudience = true,
            ValidAudience = googleClientId,
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes("JwtOwn", "JwtGoogle")
        .RequireAuthenticatedUser()
        .Build();
});

/** 
 * Comentado para evitar conflictos con la autenticación de Google.
 * Si decides usar JWT propio, descomenta esta sección y elimina la configuración de Google.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
*/

//Swagger Configuration
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Auth Bearer Token \r\n\r\n" +
        "Insert The token with the following format: Bearer thgashqkssuqj",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
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
                },
                Scheme = "oauth2",
                Name = "Bearer"
            },
            new List<string>()
        }
    });
});

// Controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// CORS configuration
var allowedOrigins = builder.Environment.IsDevelopment()
    ? new[] { "http://localhost:4200" }
    : new[] { "https://reservasaulas.com" }; // cambia por tu dominio real

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins(allowedOrigins)
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});


var app = builder.Build();

// Seeding inicial de datos
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await AppDbSeeder.SeedAsync(services);
}

// Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
