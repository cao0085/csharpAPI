using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Google.Cloud.Firestore;
using RestApiPractice.Settings;
using RestApiPractice.Providers.Firebase;

namespace RestApiPractice.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FirebaseConfigOptions>(configuration.GetSection("FirebaseConfig"));
            services.AddScoped<IFirestoreProvider, FirestoreProvider>();

            return services;
        }
    }
}