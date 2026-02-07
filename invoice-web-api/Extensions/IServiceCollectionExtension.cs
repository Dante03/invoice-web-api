using invoice_web_api.Data;
using invoice_web_api.Interfaces;
using invoice_web_api.Repositories;
using invoice_web_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;

namespace invoice_web_api.Extensions
{
    public static class IServiceCollectionExtension
    {

        public static void AddOptionsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<OptionsServices>(resolver =>
            {
                var options = new OptionsServices();
                configuration.Bind(options);
                return options;
            });
        }
        public static void AddServices(this IServiceCollection services)
        {
            services.AddHttpClient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddCorsPrivate(this IServiceCollection services)
        {

        }

        public static void AddDB(this IServiceCollection services, IConfiguration configuration)
        {
            var optionsServices = new OptionsServices();
            configuration.Bind(optionsServices);

            services.AddSingleton<OptionsServices>(optionsServices);

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(optionsServices.ConnectionString));
        }

        public static void AddJWTConfiguration(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme);
            services.AddAuthorization();
            services.AddSingleton<IConfigureOptions<JwtBearerOptions>, JwtBearerOptionsSetup>();

        }

        public static void AddSwaggerConfiguration(this  IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new()
                {
                    Title = "Invoice APIs",
                    Version = "v1"
                });
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "JWT enviado automáticamente vía cookie HttpOnly"
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
                        Array.Empty<string>()
                    }
                });
            });
        }
    }
}
