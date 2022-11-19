using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Management.HadrData;
using NuGet.ContentModel;
using WebApiTest.Models;

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();


    builder.Services.AddDbContext<ProductContext>(opt =>
        opt.UseSqlServer("Data Source=DESKTOP-UJ7JG66;Initial Catalog=MySaver;Integrated Security=True;Encrypt=False"));

    builder.Services.AddDbContext<StoreContext>(opt =>
        opt.UseSqlServer("Data Source=DESKTOP-UJ7JG66;Initial Catalog=MySaver;Integrated Security=True;Encrypt=False"));


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
