using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using CorsSettings = RestApiPractice.Settings.CorsOptions;

namespace RestApiPractice.Providers
{
    public class CorsPolicyProvider : ICorsPolicyProvider
    {
        private readonly CorsSettings _options;

        public CorsPolicyProvider(IOptions<CorsSettings> options)
        {
            _options = options.Value;
        }

        public Task<CorsPolicy?> GetPolicyAsync(HttpContext context, string? policyName)
        {
            var builder = new CorsPolicyBuilder();

            if (_options.AllowOrigins.Contains("*"))
                builder.AllowAnyOrigin();
            else
                builder.WithOrigins(_options.AllowOrigins);

            if (_options.AllowHeaders.Contains("*"))
                builder.AllowAnyHeader();
            else
                builder.WithHeaders(_options.AllowHeaders);

            if (_options.AllowMethods.Contains("*"))
                builder.AllowAnyMethod();
            else
                builder.WithMethods(_options.AllowMethods);

            if (_options.AllowCredentials)
                builder.AllowCredentials();

            var policy = builder.Build();
            return Task.FromResult<CorsPolicy?>(policy);
        }
    }
}