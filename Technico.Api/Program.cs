using Microsoft.EntityFrameworkCore;
using Technico.Api.Services;
using TechnicoRMP.DatabaseNew.DataAccess;
using TechnicoRMP.Models;
using TechnicoRMP.Shared;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DataStore>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddScoped<>

builder.Services.AddControllers();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPropertyRepairService, PropertyRepairService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



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