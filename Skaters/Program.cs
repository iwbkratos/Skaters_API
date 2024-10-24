using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Skaters.Data;
using Skaters.Mapping;
using Skaters.Models.CustomClass;
using Skaters.Repositories.AuthRepositories;
using Skaters.Repositories.CartProductRepositories;
using Skaters.Repositories.CartRepositories;
using Skaters.Repositories.ProductRepositories;
using Skaters.Repositories.StoreRepositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Skaters API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
          new OpenApiSecurityScheme
          {
           Reference=new OpenApiReference
           {
            Type=ReferenceType.SecurityScheme,
            Id=JwtBearerDefaults.AuthenticationScheme
           },
           Scheme="Oauth2",
           Name=JwtBearerDefaults.AuthenticationScheme,
           In=ParameterLocation.Header
          },
          new List<string>()
        }

    });
});
    builder.Services.AddHttpContextAccessor();
    //builder.Services.AddDbContext<SkatersDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SkatersConnectionString")));
    builder.Services.AddDbContext<SkatersAuthDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SkatersConnectionString")));
    builder.Services.AddScoped<IStoreRepository, SQLStoreRepository>();
    builder.Services.AddScoped<IProductRepository, SQLProductRepository>();
    builder.Services.AddScoped<ICartRepository, SQLCartRepository>();
    builder.Services.AddScoped<ICartProductRepository, CartProductRepository>();
    builder.Services.AddScoped<ITokenRepository, TokenRepository>();
    builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
    builder.Services.AddIdentityCore<Applicationuser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<Applicationuser>>("Skaters")
    .AddEntityFrameworkStores<SkatersAuthDbContext>()
    .AddDefaultTokenProviders();
    builder.Services.Configure<IdentityOptions>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 1;
    });


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]))
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder
                 .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});
var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseCors("AllowAllOrigins");
    app.UseAuthentication();
 

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
