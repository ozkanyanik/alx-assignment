using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace EFCore
{
    public static class EFCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddEFCore(this IServiceCollection services)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var configuration = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("efcoreSettings.json", false)
                .AddJsonFile("efcoreSettings.Development.json", true)
                .Build();
            services.Configure<EfCoreSettings>(options => configuration.Bind(options));
            var efcoreSettings = configuration.Get<EfCoreSettings>();
            services.AddDbContext<EFCoreContext>(options => options.UseSqlServer(efcoreSettings.ConnectionStrings.DevConn).UseLazyLoadingProxies());
            var key = Encoding.ASCII.GetBytes(efcoreSettings.JWTSecretKey);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
            return services;
        }
    }
}
