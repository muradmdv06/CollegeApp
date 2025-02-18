using CollegeApp.Configurations;
using CollegeApp.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CollegeApp.Data.Repository;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Configure Database Context
builder.Services.AddDbContext<CollegeDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CollegeAppDBConnection"))
);

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperConfig).Assembly);

// Register repositories
builder.Services.AddTransient<IStudentRepository, StudentRepository>();

// Configure Controllers with JSON serialization options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Enable Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// Enable Swagger UI only in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();  // Ensures correct request routing
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();
app.Run();
