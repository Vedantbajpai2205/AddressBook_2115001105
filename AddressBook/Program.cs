using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using NLog.Config;
using NLog.Web;
using NLog;
using RepositoryLayer.Context;
using System;
using AddressBook.Mapping;
using BuisnessLayer.Interface;
using BusinessLayer.Services;
using RepositoryLayer.Interface;
using RepositoryLayer.Services;
using BuisnessLayer.Mapping;
using BusinessLayer.Interface;
using BusinessLayer.Mapping;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Middleware.Authenticator;
using RepositoryLayer.Service;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Database Connection
var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
builder.Services.AddDbContext<AddressBookContext>(options => options.UseSqlServer(connectionString));
//auto mapper
builder.Services.AddAutoMapper(typeof(MappingProfileBL));
builder.Services.AddAutoMapper(typeof(UserMapper));

builder.Services.AddScoped<IUserBL, UserBL>();
builder.Services.AddScoped<IUserRL, UserRL>();
builder.Services.AddScoped<JwtTokenService>();

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSettings["Key"];

if (string.IsNullOrEmpty(jwtKey))
{
    throw new Exception("JWT Key is missing in configuration");
}

var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });


builder.Services.AddScoped<IAddressBookBL, AddressBookBL>();
builder.Services.AddScoped<IAddressBookRL, AddressBookRL>();

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Add FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<AddressBookEntryModelValidator>(); // Ensure Validator is correctly registered

//logger using nlog
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
LogManager.Configuration = new XmlLoggingConfiguration("C:\\Clone1\\AddressBook_2115001105\\nlog.config");
logger.Debug("init main");

builder.Logging.ClearProviders();
builder.Host.UseNLog();

var app = builder.Build();

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