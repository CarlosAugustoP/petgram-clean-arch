using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediatR;
using AutoMapper;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using Domain.Repositorys;
using Infrastructure.UserData;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); 
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly)); 
builder.Services.AddAutoMapper(typeof(Program)); 
var app = builder.Build();

builder.Services.AddDbContext<MainDBContext>(options => options.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=1234;"));
builder.Services.AddScoped<IUserRepository, UserRepository>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); 
}

app.UseHttpsRedirection();
app.MapControllers(); 

app.Run();
