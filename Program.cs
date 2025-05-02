
using RestApiPractice.Extensions;

using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;

using Serilog;
using Serilog.Events;

using DotNetEnv;


var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load();

var envToConfigMap = new Dictionary<string, string>
{
    { "JWT_KEY", "Jwt:Key" },
    { "JWT_ISSUER", "Jwt:Issuer" },
    { "JWT_AUDIENCE", "Jwt:Audience" },
    { "FIREBASE_PROJECT_ID", "FirebaseConfig:ProjectId" },
    { "FIREBASE_CLIENT_EMAIL", "FirebaseConfig:ClientEmail" },
    { "FIREBASE_JSON", "FirebaseConfig:ServiceAccountKeyPath" }
};

// 遍歷 map，如果環境變數有值就覆蓋 Configuration
foreach (var (envVar, configKey) in envToConfigMap)
{
    var value = Environment.GetEnvironmentVariable(envVar);
    if (!string.IsNullOrEmpty(value))
    {
        builder.Configuration[configKey] = value;
    }
}



// Log Setting
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.File(
        path: "./logs/all-.log",
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] ({SourceContext}) {Message:lj}{NewLine}{Exception}",
        rollingInterval: RollingInterval.Hour,
        rollOnFileSizeLimit: true,
        retainedFileTimeLimit: TimeSpan.FromDays(7), // 檔案保留天數
        fileSizeLimitBytes: 10 * 1024 * 1024
    )// 僅寫入 ERROR 層級的日誌到另一個檔案
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Error)
        .WriteTo.File("logs/error-.log",
            rollingInterval: RollingInterval.Hour,
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] ({SourceContext}) {Message:lj}{NewLine}{Exception}",
            retainedFileTimeLimit: TimeSpan.FromDays(7) // 檔案保留天數
        )
    )
    .CreateLogger();
builder.Host.UseSerilog();


// Docker or PORT-bound hosting
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(int.Parse(port));
    });
}

// start setting main container
builder.Services.AddControllers();
builder.Services.AddProjectServices(builder.Configuration);


builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5001); // 改用 5001 或其他不衝突的 port
});

// 加入 Swagger（方便測 API）
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// app build
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 有順序之分
app.UseCors(); 

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
