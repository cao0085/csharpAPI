using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors.Infrastructure;

using Google.Cloud.Firestore;
using CorsSettings = RestApiPractice.Settings.CorsOptions;

using RestApiPractice.Providers;
using RestApiPractice.Settings;



namespace RestApiPractice.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services, IConfiguration configuration)
        {   

            services.Configure<CorsSettings>(configuration.GetSection("Cors"));
            services.AddSingleton<ICorsPolicyProvider, CorsPolicyProvider>();
            services.AddCors();

            
            services.Configure<FirebaseConfigOptions>(configuration.GetSection("FirebaseConfig"));
            services.AddScoped<IFirestoreProvider, FirestoreProvider>();

            return services;
        }
    }
}