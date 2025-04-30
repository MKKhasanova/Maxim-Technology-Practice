using Microsoft.OpenApi.Models;
using StringProcessor.API.Services;
using StringProcessor.API.Services.Interfaces;
using StringProcessor.API.Utilities;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "String Processor API",
        Version = "v1",
        Description = "API для обработки строк"
    });
});

// Register services
builder.Services.AddScoped<IStringProcessorService, StringProcessorService>();
builder.Services.AddScoped<IRandomNumberGenerator, RandomNumberGenerator>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
