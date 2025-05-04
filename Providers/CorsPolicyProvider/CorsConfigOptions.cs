namespace RestApiPractice.Providers
{
    public class CorsConfigOptions
    {
        public string[] AllowOrigins { get; set; } = Array.Empty<string>();
        public string[] AllowHeaders { get; set; } = Array.Empty<string>();
        public string[] AllowMethods { get; set; } = Array.Empty<string>();
        public bool AllowCredentials { get; set; } = false;
    }
}