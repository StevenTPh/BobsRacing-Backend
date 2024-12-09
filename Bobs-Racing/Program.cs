using Bobs_Racing.Repositories;
using Bobs_Racing.Data; // Ensure you include your AppDbContext namespace
using Microsoft.EntityFrameworkCore;
using Bobs_Racing.Interface;
using Bobs_Racing.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register AppDbContext with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<IRaceRepository, RaceRepository>();
builder.Services.AddScoped<IAthleteRepository, AthleteRepository>();
builder.Services.AddScoped<IRaceAthleteRepository, RaceAthleteRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBetRepository, BetRepository>();
builder.Services.AddScoped<IRaceService, RaceService>();

// Add controllers
builder.Services.AddControllers();

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

////////// Slik at frontenden kan sende http forespørsler (API Calls) til backenden - Enock
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost/5173") // Replace with your frontend URL
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

var app = builder.Build();


app.UseCors();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

