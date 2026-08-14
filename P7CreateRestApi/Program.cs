using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.IRepositories;
using P7CreateRestApi.Repositories;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container. 
builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddJwtBearer();
builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add IRepository

builder.Services.AddScoped<IBidListRepository, BidListRepository>();
builder.Services.AddScoped<ICurvePointRepository, CurvePointRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IRuleNameRepository, RuleNameRepository>();
builder.Services.AddScoped<ITradeRepository, TradeRepository>();
//builder.Services.AddScoped<SignInManager<User>, SignInManager<User>>();
//builder.Services.AddScoped<UserManager<User>, UserManager<User>>();


builder.Services.AddDbContext<P7Referential>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("P7Referential")));

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("P7Identity")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

//app.MapIdentityApi<IdentityUser>();

app.MapGet("/secret", (ClaimsPrincipal user) => $"Hello {user.Identity?.Name}. My secret")
    .RequireAuthorization();

app.MapGet("/BidLists", (ClaimsPrincipal user) => $"Hello {user.Identity?.Name}. My BidList")
    .RequireAuthorization();

app.Run();
