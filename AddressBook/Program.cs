using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using System;

var builder = WebApplication.CreateBuilder(args);

// Database Connection
var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
builder.Services.AddDbContext<AddressContext>(options => options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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