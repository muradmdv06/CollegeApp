using CollegeApp.Configurations;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.Data.Repo;
using CollegeApp.MyLogging;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Text.Json.Serialization;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CollegeApp.Services;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Use Serilog
builder.Host.UseSerilog();

try
{
    // Get Connection String
    var connectionString = builder.Configuration.GetConnectionString("CollegeAppDBConnection");
    if (string.IsNullOrEmpty(connectionString))
    {
        Log.Fatal("Database connection string is missing!");
        throw new InvalidOperationException("Database connection string is missing!");
    }

    // Logging
    builder.Services.AddTransient<IMyLogger, LogToServerMemory>();

    // Database
    builder.Services.AddDbContext<CollegeDBContext>(options =>
        options.UseSqlServer(connectionString));

    // Automapper
    builder.Services.AddAutoMapper(typeof(AutoMapperConfig).Assembly);

    // Repositories
    builder.Services.AddScoped<IStudentRepository, StudentRepository>();
    builder.Services.AddScoped<IUserService,UserService>();
    builder.Services.AddScoped(typeof(ICollegeRepository<>), typeof(CollegeRepository<>));

    // Controllers & JSON Configurations
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

    // Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // CORS Configuration
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader());
    });
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    });

    var app = builder.Build();

    // Middleware
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseCors("AllowAll");  // Ensure the correct CORS policy name
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application startup failed!");
}
finally
{
    Log.CloseAndFlush();
}


