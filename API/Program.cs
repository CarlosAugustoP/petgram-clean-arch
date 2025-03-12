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
using Microsoft.OpenApi.Models;
using Application.Abstractions.Posts.CreatePostCommand;
using Infrastructure.PostData;
using Infrastructure.MediaData;
using Infrastructure.LikeData;
using Infrastructure.CommentData;
using Application.Abstractions.Followers.GetFollowers;
using Application.Abstractions.Followers.GetFollowingByUser;
using Application.Abstractions.Likes.GetLikesByPostQuery;
using Application.Abstractions.Likes.LikePostCommand;
using Application.Abstractions.Posts.GetPostByIdQuery;
using Application.Abstractions.Comments.CreateCommentCommand;
using StackExchange.Redis;
using Application.Abstractions.Users.ValidateToken;
using Application.Abstractions.Comments.DeleteCommentCommand;
using Application.Abstractions.Comments.CreateReplyCommand;
using Application.Abstractions.Comments.GetCommentsFromPostQuery;
using Application.Abstractions.Comments.LikeCommentCommand;
using Application.Abstractions.Comments.UpdateCommentCommand;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

#region[Services]
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
builder.Services.AddScoped<IPasswordHasher, PasswordHelper>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILikeRepository, LikeRepository>();
builder.Services.AddScoped<IMediaRepository, MediaRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ProfanityFilter.ProfanityFilter>();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(AddNewUserCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(StartFollowingCommand).Assembly); 
});
builder.Services.AddValidatorsFromAssembly(typeof(AddNewUserCommandValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(CreatePostCommandValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(LoginCommandValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(StartFollowingCommandValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(CreatePostCommandValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(CreateCommentCommandValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(GetFollowingByUserQueryValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(GetLikesByPostQueryValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(LikePostCommandValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(GetPostByIdQueryValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(ValidateTokenCommandValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(GetFollowersByUserQueryValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(DeleteCommentCommandValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(CreateReplyCommandValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(GetCommentsFromPostQueryValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(LikeCommentCommandValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(UpdateCommentCommandValidator).Assembly);


var smtpKey = Environment.GetEnvironmentVariable("SMTP_KEY") ?? throw new ArgumentNullException("Invalid smtp key");
builder.Services.AddSingleton<IEmailService, EmailService>(ems => new EmailService(smtpKey));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddAuthorization();

var supabaseKey = Environment.GetEnvironmentVariable("SUPABASE_KEY") ?? throw new ArgumentNullException("SUPABASE_KEY");
var supabaseUrl = Environment.GetEnvironmentVariable("SUPABASE_URL") ?? throw new ArgumentNullException("SUPABASE_URL");
builder.Services.AddSingleton<ISupabaseService, SupabaseService>(sup => new SupabaseService(supabaseKey, supabaseUrl));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insert the JWT Token created during login",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

#endregion

#region[DataBase]
var localhost = Environment.GetEnvironmentVariable("DB_HOST") ?? throw new ArgumentNullException("DB_HOST");
var port = Environment.GetEnvironmentVariable("DB_PORT") ?? throw new ArgumentNullException("DB_PORT");
var database = Environment.GetEnvironmentVariable("DB_DATABASE") ?? throw new ArgumentNullException("DB_DATABASE");
var username = Environment.GetEnvironmentVariable("DB_USERNAME") ?? throw new ArgumentNullException("DB_USERNAME");
var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? throw new ArgumentNullException("DB_PASSWORD");



var connectionString = $"Host={localhost};Port={port};Database={database};Username={username};Password={password};";
builder.Services.AddDbContext<MainDBContext>(options => 
    options.UseNpgsql(connectionString));
#endregion    

#region [Redis]
var pass = Environment.GetEnvironmentVariable("REDIS_PASSWORD") ?? throw new ArgumentNullException("Could not find redis variable");
builder.Services.AddSingleton(ConnectionMultiplexer.Connect($"localhost:6379, password={pass}"));
builder.Services.AddScoped<IRedisService, RedisService>();
#endregion

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

#region[Middleware]
app.UseMiddleware<ValidationExceptionMiddleware>();
app.UseMiddleware<CustomExceptionsCatchingMiddleware>();
app.UseMiddleware<UserValidationMiddleware>();
#endregion

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
