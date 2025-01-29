using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediatR;
using AutoMapper;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using Domain.Repositorys;
using Infrastructure.UserData;
using Microsoft.AspNetCore.Identity;
using Application.Abstractions.Users.AddNewUser;
using Application.Abstractions.Followers.StartFollowing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(AddNewUserCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(StartFollowingCommand).Assembly); 
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "API", Version = "v1" });
});

var localhost = Environment.GetEnvironmentVariable("DB_HOST") ?? throw new ArgumentNullException("DB_HOST");
var port = Environment.GetEnvironmentVariable("DB_PORT") ?? throw new ArgumentNullException("DB_PORT");
var database = Environment.GetEnvironmentVariable("DB_DATABASE") ?? throw new ArgumentNullException("DB_DATABASE");
var username = Environment.GetEnvironmentVariable("DB_USERNAME") ?? throw new ArgumentNullException("DB_USERNAME");
var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? throw new ArgumentNullException("DB_PASSWORD");

builder.Services.AddDbContext<MainDBContext>(options =>
    options.UseNpgsql($"Host={localhost};Port={port};Database={database};Username={username};Password={password};"));
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));

}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
