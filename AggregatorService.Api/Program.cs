using AggregatorService.Application.Queries;
using AggregatorService.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// add jwt authentication.
var validKey = builder.Configuration.GetValue<string>("JwtSettings:Key");
var validIssuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer");
var validAudience = builder.Configuration.GetValue<string>("JwtSettings:Audience");
var validExpiresIn = builder.Configuration.GetValue<int>("JwtSettings:ExpiryMinutes");

builder.Services.AddJWT(validKey, validIssuer, validAudience, validExpiresIn);
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
        ValidIssuer = validIssuer,
        ValidAudience = validAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(validKey))
    };
});

builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Add JWT Auth support
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Enter JWT token in format: Bearer {token}",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

// add news api client.
var newsApiKey = builder.Configuration.GetValue<string>("ApiKeys:NewsApi");
builder.Services.AddNewsService(newsApiKey);

var weatherApiKey = builder.Configuration.GetValue<string>("ApiKeys:OpenWeatherApi");
builder.Services.AddWeatherService(weatherApiKey);
builder.Services.AddAggregateDbContextAndRepo();
builder.Services.AddMemoryCache();

builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<FetchNewsQuery>());
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<AggregateQueryHandler>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
