using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;
using Lms.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<LmsDbContext>(optionsAction =>
    optionsAction.UseSqlServer(builder.Configuration.GetConnectionString("LmsDbContext")));

var app = builder.Build();

//Seed Data
app.SeedDataAsync().GetAwaiter().GetResult();

// Configure the HTTP request pipeline.


app.Run();