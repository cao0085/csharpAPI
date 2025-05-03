using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors.Infrastructure;

using Google.Cloud.Firestore;
using CorsSettings = RestApiPractice.Settings.CorsOptions;

using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using RestApiPractice.Extensions;
using System.Text;

using RestApiPractice.Providers;
using RestApiPractice.Settings;

using RestApiPractice.LogicLayer;
using RestApiPractice.Repositories;


namespace RestApiPractice.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services, IConfiguration configuration)
        {   

            services.Configure<RestApiPractice.Settings.CorsOptions>(configuration.GetSection("Cors"));
            services.AddSingleton<ICorsPolicyProvider, CorsPolicyProvider>();
            services.AddCors();

            
            services.Configure<FirebaseConfigOptions>(configuration.GetSection("FirebaseConfig"));
            services.AddScoped<IFirebaseProvider,FirestoreProvider>();


            services.AddScoped<JwtService>();
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                    };
                });
            services.AddAuthorization();

            services.AddScoped<GoogleLoginLogic>();
            services.AddScoped<SpotifyLoginLogic>();
            services.AddScoped<AccountLogic>();
            services.AddScoped<AccountRepository>();

            return services;
        }
    }
}