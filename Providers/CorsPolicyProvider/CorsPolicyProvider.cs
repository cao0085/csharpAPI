using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace RestApiPractice.Providers
{   
    // ICorsPolicyProvider by Microsoft.AspNetCore.Cors.Infrastructure
    public class CorsPolicyProvider : ICorsPolicyProvider
    {
        private readonly CorsConfigOptions _config;

        public CorsPolicyProvider(IOptions<CorsConfigOptions> config)
        {
            _config = config.Value;
        }

        public Task<CorsPolicy?> GetPolicyAsync(HttpContext context, string? policyName)
        {
            var builder = new CorsPolicyBuilder();

            if (_config.AllowOrigins.Contains("*"))
                builder.AllowAnyOrigin();
            else
                builder.WithOrigins(_config.AllowOrigins);

            if (_config.AllowHeaders.Contains("*"))
                builder.AllowAnyHeader();
            else
                builder.WithHeaders(_config.AllowHeaders);

            if (_config.AllowMethods.Contains("*"))
                builder.AllowAnyMethod();
            else
                builder.WithMethods(_config.AllowMethods);

            if (_config.AllowCredentials)
                builder.AllowCredentials();

            var policy = builder.Build();
            return Task.FromResult<CorsPolicy?>(policy);
        }
    }
}