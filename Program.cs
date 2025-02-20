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


var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    
    var connectionString = builder.Configuration.GetConnectionString("CollegeAppDBConnection");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Database connection string is missing!");
    }

    
    builder.Services.AddDbContext<CollegeDBContext>(options =>
        options.UseSqlServer(connectionString)
    );

    
    builder.Services.AddAutoMapper(typeof(AutoMapperConfig).Assembly);

    
    builder.Services.AddTransient<IMyLogger, LogToServerMemory>();
    builder.Services.AddScoped<IStudentRepository, StudentRepository>();
    builder.Services.AddScoped(typeof(ICollegeRepository<>), typeof(CollegeRepository<>));

    
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

   
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader());
    });

    var app = builder.Build();

    
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseCors("AllowAll");
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

