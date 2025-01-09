using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NewEraCashCarry.Helpers;
using NewEraCashCarry.Interfaces;
using NewEraCashCarry.Middlewares;
using NewEraCashCarry.Services;
using PDP.University.Examine.Project.Web.API.Data;
using PDP.University.Examine.Project.Web.API.Interfaces;
using PDP.University.Examine.Project.Web.API.Repositories;
using Serilog;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using NewEraCashCarry.Repositories;

namespace PDP.University.Examine.Project.Web.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(@"E:\Errors\logs\log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();


            builder.Services.AddDbContext<ApplicationDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IImageRepository, ImageRepository>();

            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                    policy.RequireRole("Admin"));
            });

            builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));

            var authSettings = builder.Configuration.GetSection("AuthSettings").Get<AuthSettings>();
            builder.Services.AddSingleton(authSettings);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Secret)),

                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "NewEraCashCarry API",
                    Version = "v1",
                    Description = "API for managing a NewEraCashCarry platform (v1.0)"
                });
                c.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "2.0",
                    Title = "NewEraCashCarry API",
                    Description = "API for managing a NewEraCashCarry platform (v2.0)"
                });

                // Add JWT to Swagger
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter JWT token (Example: Bearer {token})",
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
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
                        new string[] {}
                    }
                });
            });

            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                    {
                        options.SwaggerEndpoint("/swagger/v1/swagger.json", "NewEraCashCarry API v1.0");
                        options.SwaggerEndpoint("/swagger/v2/swagger.json", "NewEraCashCarry API v2.0");
                    });
            }

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
