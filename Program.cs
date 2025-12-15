using ClinicalBackend.Data;
using ClinicalBackend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ... باقي الخدمات

builder.Services.AddEndpointsApiExplorer();  // لازم قبل AddSwaggerGen
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ... باقي الـ middleware


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();