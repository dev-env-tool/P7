using AutoMapper.Configuration;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.IRepositories;
using P7CreateRestApi.IServices;
using P7CreateRestApi.LogUserNameMiddleware;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Services;
using Serilog;
using System;
using System.Security.Claims;
using System.Text;

//using Serilog.Sinks;
//using Serilog.Sinks.File;
//using Serilog.Enrichers;

//using P7CreateRestApi.LogUserNameMiddleware;





var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container. 
builder.Services.AddAuthorization();
//builder.Services.AddAuthentication("Bearer").AddJwtBearer();
builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
//---------------------------------------------------------------------
//builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
//---------------------------------------------------------------------

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "jwtToken_Auth_API",
        Version = "v1"
    });
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Token bearer[space] token",
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
            }
        });
});


//Add IRepository

builder.Services.AddScoped<IBidListRepository, BidListRepository>();
builder.Services.AddScoped<ICurvePointRepository, CurvePointRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IRuleNameRepository, RuleNameRepository>();
builder.Services.AddScoped<ITradeRepository, TradeRepository>();
builder.Services.AddScoped<ILoginService, LoginService>();
//builder.Services.AddScoped<SignInManager<User>, SignInManager<User>>();
//builder.Services.AddScoped<UserManager<User>, UserManager<User>>();
//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<P7Referential>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("P7Referential")));

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("P7Identity")));


//-----------------------JWT Token configuration----------------------------------------

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        //-----------------------Issuer Audience and key come from appsetting.Json------------------------------------
        //-----------------------Issuer and aAudience both refer to the local host http Address-----------------------
        ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
        //-----------------------The kew was generated via a new Guid it helps authentify the server once called------
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value))
    };
}
);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));








var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();



app.UseAuthentication();
app.UseAuthorization();

//-----------------------Serilog intitial configuration----------------------------------------
//using var log = new LoggerConfiguration()
//    .CreateLogger();

//log.Information("Hello, Serilog!");
//Log.Logger = log;
//Log.Information("The global logger has been configured");

//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Debug()
//    .Enrich.WithRequestUserId()
//    .WriteTo.File("logs/P7CreateRestApi.txt", rollingInterval: RollingInterval.Month)
//    .CreateLogger();

//Log.Information("The global log file has been configured");





//-----------------------Calling middleware helps each log to get signed in userName-----------
//app.UseMiddleware<LogUserNameMiddleware>();

//-----------------------Functions working as endpoint shortcuts-------------------------------
//app.MapIdentityApi<IdentityUser>();

//app.MapGet("/secret", (ClaimsPrincipal user) => $"Hello {user.Identity?.Name}. My secret")
//    .RequireAuthorization();

//app.MapGet("/BidLists", (ClaimsPrincipal user) => $"Hello {user.Identity?.Name}. My BidList")
//    .RequireAuthorization();
//---------------------------------------------------------------------------------------------

app.UseMiddleware<LogUserNameMiddleware>();
app.UseSerilogRequestLogging();
app.Run();

//---------------------------------------------------------------------------------------------

