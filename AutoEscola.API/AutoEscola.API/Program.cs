using AutoEscola.API.BLL;
using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;



var builder = WebApplication.CreateBuilder(args);


var key = builder.Configuration["Jwt:Key"];

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "YouDrive.API", Version = "v1" });

    //Configuração JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite: Bearer {seu token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});



// DATABASE
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Adicionando Liberação de Cors por URL
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddScoped<JwtService>();
builder.Services.AddHttpContextAccessor();

//TABELAS BANCO PARA INJEÇÃO DE DEPENDENCIA
builder.Services.AddScoped<IUsuarios, UsuariosBLL>();
builder.Services.AddScoped<IAulas, AulasBLL>();
builder.Services.AddScoped<IInstrutor, InstrutorBLL>();
builder.Services.AddScoped<IDocumento, DocumentoBLL>(); 
builder.Services.AddScoped<IStorage, StorageBLL>();
builder.Services.AddScoped<ITiposDocumento, TiposDocumentoBLL>();
builder.Services.AddScoped<IGrupo, GrupoBLL>();
builder.Services.AddScoped<IGrupoUsuario, GrupoUsuarioBLL>();
builder.Services.AddScoped<IVeiculo, VeiculoBLL>();
builder.Services.AddScoped<IEndereco, EnderecoBLL>();
builder.Services.AddScoped<ICartao, CartaoBLL>();

var app = builder.Build();

app.UseCors("AllowReact");


app.UseAuthentication();
app.UseAuthorization();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
