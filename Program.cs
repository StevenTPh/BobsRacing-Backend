using Bobs_Racing;
using Bobs_Racing.Interface;
using Bobs_Racing.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Access the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("RacingDbConnection");

// Register the IUserRepository and its implementation (UserRepository)
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddControllers();
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(connectionString));  // Use SQL Server
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("The connection string is not set correctly.");
}

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the IUserRepository and its implementation (UserRepository)
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

/*//////////* Slik at frontenden kan sende http forespørsler (API Calls) til backenden - Enock
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Replace with your frontend URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

app.UseCors();
///////////**/


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
