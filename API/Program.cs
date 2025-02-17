using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using Domain.Repositorys;
using Infrastructure.UserData;
using Application.Abstractions.Users.AddNewUser;
using Application.Abstractions.Followers.StartFollowing;
using Application.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Services;
using System.Reflection;
using FluentValidation;
using MediatR;
using API.Abstractions.Helpers;
using Application.Abstractions.Users.Login;
using API.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IPasswordHasher, PasswordHelper>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(AddNewUserCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(StartFollowingCommand).Assembly); 
});
builder.Services.AddValidatorsFromAssembly(typeof(AddNewUserCommandValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(LoginCommandValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(StartFollowingCommandValidator).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "API", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
var localhost = Environment.GetEnvironmentVariable("DB_HOST") ?? throw new ArgumentNullException("DB_HOST");
var port = Environment.GetEnvironmentVariable("DB_PORT") ?? throw new ArgumentNullException("DB_PORT");
var database = Environment.GetEnvironmentVariable("DB_DATABASE") ?? throw new ArgumentNullException("DB_DATABASE");
var username = Environment.GetEnvironmentVariable("DB_USERNAME") ?? throw new ArgumentNullException("DB_USERNAME");
var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? throw new ArgumentNullException("DB_PASSWORD");



var connectionString = $"Host={localhost};Port={port};Database={database};Username={username};Password={password};";
builder.Services.AddDbContext<MainDBContext>(options => 
    options.UseNpgsql(connectionString));
    
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<MainDBContext>();
        dbContext.Database.Migrate(); 

    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));

}
app.UseMiddleware<ValidationExceptionMiddleware>();
app.UseMiddleware<CustomExceptionsCatchingMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
