using CollegeApp.Configurations;
using CollegeApp.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();

builder.Logging.AddLog4Net();
builder.Services.AddDbContext<CollegeDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CollegeAppDBConnection"));
});

#region Serilog Settings
//Log.Logger = new LoggerConfiguration().
//    MinimumLevel.Information()
//    .WriteTo.File("Log/log.txt",
//    rollingInterval:RollingInterval.Minute)
//    .CreateLogger();

//builder.Host.UseSerilog();

#endregion 

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{

    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(AutoMapperConfig));



// Enable CORS
builder.Services.AddCors(options => 
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// Enable Swagger only in Development mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware configuration
app.UseHttpsRedirection();

app.UseCors("AllowAll"); // Enable CORS

app.UseAuthorization();

app.MapControllers();

app.Run();