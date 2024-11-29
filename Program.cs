using Bobs_Racing.Repositories;
using Bobs_Racing.Data; // Ensure you include your AppDbContext namespace
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register AppDbContext with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<IRaceRepository, RaceRepository>();
builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();

// Add controllers
builder.Services.AddControllers();

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//////////* Slik at frontenden kan sende http forespørsler (API Calls) til backenden - Enock
/*builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Replace with your frontend URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

app.UseCors();*/
///////////*
///

var app = builder.Build();

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

