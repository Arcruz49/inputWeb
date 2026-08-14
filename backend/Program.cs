using System.Text;
using InputWeb.Application.Interfaces;
using InputWeb.Application.Security;
using InputWeb.Application.UseCases;
using InputWeb.Domain.Entities;
using InputWeb.Domain.Interfaces;
using InputWeb.Infrastructure.Data;
using InputWeb.Infrastructure.Middlewares;
using InputWeb.Infrastructure.Repositories;
using InputWeb.Infrastructure.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Gravações de tela passam fácil dos ~28 MB padrão do Kestrel; sem isso o upload volta 413.
var maxUploadBytes = builder.Configuration.GetValue<long?>("Upload:MaxBytes") ?? 2L * 1024 * 1024 * 1024;

builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = maxUploadBytes);
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = maxUploadBytes;
    options.MultipartHeadersLengthLimit = int.MaxValue;
});

builder.Services.AddDbContext<Context>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

//common
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IFileStorage, AzureBlobStorage>();
builder.Services.AddScoped<JwtTokenGenerator>();
builder.Services.AddScoped<PasswordHasher<User>>();

//repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRecordRepository, RecordRepository>();

//usecases
builder.Services.AddScoped<IRegisterUserUseCase, RegisterUseCase>();
builder.Services.AddScoped<IAuthenticateUseCase, AuthenticateUseCase>();
builder.Services.AddScoped<ICreateRecordingUseCase, CreateRecordingUseCase>();
builder.Services.AddScoped<IGetRecordByIdUseCase, GetRecordByIdUseCase>();
builder.Services.AddScoped<IGetRecordsUseCase, GetRecordsUseCase>();


var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtKey))
    throw new Exception("JWT Key not configured");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = ctx =>
            {
                ctx.Token = ctx.Request.Cookies["inputweb_token"];
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.ConfigObject.AdditionalItems["withCredentials"] = true;
    });
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
// app.UseRateLimiter();
app.MapControllers();

app.Run();
