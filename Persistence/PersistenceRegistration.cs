using Application.Contracts.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistence.DatabaseContext;
using Persistence.Repositories;

namespace Persistence {
    public static class PersistenceRegistration {
        public static IServiceCollection AddPersistenceService(this IServiceCollection services,
            string connection,
            string jwtIssuer,
            string jwtAudience,
            string jwtKey) {


            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));
            services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(connection));

            services.AddIdentityConfiguration();
            services.AddAuthenticationWithJwt(jwtIssuer, jwtAudience, jwtKey);


            services.AddScoped<ICategoryRepository, CategoryRepository>();

            return services;
        }

        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services) {
            services.AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("bootcamp")
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });

            return services;
        }

        public static IServiceCollection AddAuthenticationWithJwt(this IServiceCollection services,
            string jwtIssuer,
            string jwtAudience,
            string jwtKey) {
            var signingKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters {
                        AuthenticationType = "Jwt",
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,
                        IssuerSigningKey = signingKey
                    };
                });
            
            return services;
        }
    }
}
