using AutoMapper;
using AutoMapper.Configuration;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using P7CreateRestApi;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.DTO;
using P7CreateRestApi.Filters;
using P7CreateRestApi.IRepositories;
using P7CreateRestApi.IServices;
using P7CreateRestApi.LogUserNameMiddleware;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Services;
using Serilog;
using Serilog.Core;
using System;
using System.Runtime;
using System.Security.Claims;
using System.Text;




public class Program
{
    public static  async Task Main(string[] args)
    { 
        var builder = WebApplication.CreateBuilder(args);
        ConfigurationManager configuration = builder.Configuration;


        builder.Services.AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();




        builder.Services.Configure<IdentityOptions>(options =>
        {
            // Default Password settings.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 10;
            options.Password.RequiredUniqueChars = 1;
        });


        builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });


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





        builder.Services.AddScoped<AsyncActionFilter>();
        builder.Services.AddAutoMapper(configAction => configAction.CreateMap<BidList, BidListDto>());
        builder.Services.AddAutoMapper(configAction => configAction.CreateMap<BidListDto, BidList>());
        builder.Services.AddAutoMapper(configAction => configAction.CreateMap<CurvePoint, CurvePointDto>());
        builder.Services.AddAutoMapper(configAction => configAction.CreateMap<CurvePointDto, CurvePoint>());
        builder.Services.AddAutoMapper(configAction => configAction.CreateMap<Rating, RatingDto>());
        builder.Services.AddAutoMapper(configAction => configAction.CreateMap<RatingDto, Rating>());
        builder.Services.AddAutoMapper(configAction => configAction.CreateMap<RuleName, RuleNameDto>());
        builder.Services.AddAutoMapper(configAction => configAction.CreateMap<RuleNameDto, RuleName>());
        builder.Services.AddAutoMapper(configAction => configAction.CreateMap<Trade, TradeDto>());
        builder.Services.AddAutoMapper(configAction => configAction.CreateMap<TradeDto, Trade>());

        builder.Services.AddScoped<IBidListRepository, BidListRepository>();
        builder.Services.AddScoped<IBidListService, BidListService>();
        builder.Services.AddScoped<ICurvePointRepository, CurvePointRepository>();
        builder.Services.AddScoped<ICurvePointService, CurvePointService>();
        builder.Services.AddScoped<IRatingRepository, RatingRepository>();
        builder.Services.AddScoped<IRatingService, RatingService>();
        builder.Services.AddScoped<IRuleNameRepository, RuleNameRepository>();
        builder.Services.AddScoped<IRuleNameService, RuleNameService>();
        builder.Services.AddScoped<ITradeRepository, TradeRepository>();
        builder.Services.AddScoped<ITradeService, TradeService>();
        builder.Services.AddScoped<ILoginService, LoginService>();
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
                RoleClaimType = ClaimTypes.Role,
                //RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role:",
                ValidateIssuer = true,
                ValidateAudience = true,
                RequireExpirationTime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
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

        builder.Services.AddAuthorizationBuilder();


        var app = builder.Build();
        //app.UseStaticFiles();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();


        app.UseMiddleware<LogUserNameMiddleware>();


        app.UseSerilogRequestLogging();

        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new[] { "Admin", "Member" };

            foreach (var role in roles)
            {

                if(!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            string userAdminEmail = "admin@user.com";
            string userAdminPassword = "Passadmin,123";
            const string role = "Admin";

            if (await userManager.FindByEmailAsync(userAdminEmail) == null)
            {
                var userAdmin = new User();
                userAdmin.UserName = userAdminEmail;
                userAdmin.Email = userAdminEmail;

                await userManager.CreateAsync(userAdmin, userAdminPassword);
                await userManager.AddToRoleAsync(userAdmin, role);
            }

        }

        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            string userMemberEmail = "member@user.com";
            string userMemberPassword = "Passmember,123";
            const string role = "Member";

            if (await userManager.FindByEmailAsync(userMemberEmail) == null)
            {
                var userMember = new User();
                userMember.UserName = userMemberEmail;
                userMember.Email = userMemberEmail;

                await userManager.CreateAsync(userMember, userMemberPassword);
                await userManager.AddToRoleAsync(userMember, role);
            }
        }


        app.Run();
    }
}


