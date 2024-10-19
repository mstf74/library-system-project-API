
using Microsoft.EntityFrameworkCore;
using DataAcessLayer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.Text;
using DataAcessLayer.GenericRepo;
using BusinessLayer.ManagersInterfaces;
using BusinessLayer.Managers;
using Microsoft.OpenApi.Models;

namespace library_system_project_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["jwt:issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["jwt:audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:key"]))
                };
            });
            builder.Services.AddAuthorization();
            builder.Services.AddSwaggerGen(c =>
            {
                // Add security definition
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                // Add security requirement
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
            new string[] {}
        }
    });
            });
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<Context>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DEPI_Test"));
            });
            builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options => {
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.";
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<Context>();
            builder.Services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
            builder.Services.AddScoped<IAccountManager,AccountManager>();
            builder.Services.AddScoped(typeof(UserManager<>));
            




            var app = builder.Build();

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

            app.Run();
        }
    }
}
