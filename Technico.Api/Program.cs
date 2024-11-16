using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Technico.Api.Logger;
using Technico.Api.Services;
using Technico.Api.Validations;
using TechnicoRMP.Database.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataStore>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddScoped<>

builder.Services.AddControllers();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPropertyItemService, PropertyItemService>();
builder.Services.AddScoped<IPropertyRepairService, PropertyRepairService>();
builder.Services.AddScoped<IPropertyItemValidation, PropertyItemValidation>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Logging.AddProvider(new DbLoggerProvider(builder.Services.BuildServiceProvider()
    .GetRequiredService<DataStore>()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors((opts) =>
{
    opts.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();