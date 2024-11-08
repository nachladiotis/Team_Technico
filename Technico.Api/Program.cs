using Microsoft.EntityFrameworkCore;
using Technico.Api.Services;
using TechnicoRMP.Database.DataAccess;
using TechnicoRMP.Models;
using TechnicoRMP.Shared;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddDbContext<DataStore>(options =>
    options.UseSqlServer("Data Source=(local);Initial Catalog=TechnicoDb;Integrated Security=True;TrustServerCertificate=True"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
