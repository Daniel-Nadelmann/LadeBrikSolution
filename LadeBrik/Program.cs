using LadeBrik.Database;
using LadeBrik.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ILadeBrikService, LadeBrikService>();
var connectionString = builder.Configuration.GetConnectionString("LadeBrikDb") ??
                       Environment.GetEnvironmentVariable("LadeBrikDb");

builder.Services.AddDbContext<LadeBrikDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LadeBrikDb")));


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
