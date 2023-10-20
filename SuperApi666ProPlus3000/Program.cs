using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuperApi666ProPlus3000.BackendModels;
using SuperApi666ProPlus3000.DbContexts;
using SuperApi666ProPlus3000;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//!
builder.Services.AddDbContext<MainDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("MainDb"))
    );
//!
builder.Services.AddMyAuth();

var app = builder.Build();

//!
await app.Services.SynchronizeIdentityRoles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//!
//1) ауфентикация
//2) авторизация
app.UseAuthorization();
app.UseAuthentication();



app.MapControllers();

app.Run();