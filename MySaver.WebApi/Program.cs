using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System;
using WebApiTest.Middleware;
using WebApiTest.Models;
using Serilog;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("Logs/ErrorLog-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

string rootDir = builder.Environment.ContentRootPath;
string connectionString = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = " + rootDir 
    + "\\Data\\MySaver.mdf; Initial Catalog=MySaver; Integrated Security = True; Connect Timeout = 30";

builder.Services.AddDbContext<ProductContext>(opt =>
    opt.UseSqlServer(connectionString));

builder.Services.AddDbContext<StoreContext>(opt =>
    opt.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MiddlewareExamples v1"));
}
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
