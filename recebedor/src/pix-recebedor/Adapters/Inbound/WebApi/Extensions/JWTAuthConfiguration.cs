using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Adapters.Inbound.WebApi.Extensions
{
    public static class JWTAuthConfiguration
    {

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = false,
                     ValidateAudience = false,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     //ValidIssuer = configuration["Jwt:Issuer"] ?? "pix-recebedor",
                     //ValidAudience = configuration["Jwt:Audience"] ?? "pix-recebedor",
                     IssuerSigningKey = new SymmetricSecurityKey(
                         Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? "dAWG7KP2xpHPN8aU1GfC82OkOqwXSz5w"))
                 };
             });

            services.AddAuthorization();

            return services;
        }
    }
}
