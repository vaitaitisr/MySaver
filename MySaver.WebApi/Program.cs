using Microsoft.EntityFrameworkCore;
using System;
using WebApiTest.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

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
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
