using AggregatorService.Application.Queries;
using AggregatorService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add news api client.
var newsApiKey = builder.Configuration.GetValue<string>("ApiKeys:NewsApi");
builder.Services.AddNewsService(newsApiKey);
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

app.UseAuthorization();

app.MapControllers();

app.Run();
