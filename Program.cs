
using RestApiPractice.Extensions;

using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;

using Serilog;
using Serilog.Events;

using DotNetEnv;


var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;


if (env.IsDevelopment())
{
    DotNetEnv.Env.Load();
}else
{
    Console.WriteLine("=== Production Environment ===");
}


var envToConfigMap = new Dictionary<string, string>
{
    { "JWT_KEY", "Jwt:Key" },
    { "JWT_ISSUER", "Jwt:Issuer" },
    { "JWT_AUDIENCE", "Jwt:Audience" },
    { "FIREBASE_PROJECT_ID", "FirebaseConfig:ProjectId" },
    { "FIREBASE_KEY_PATH", "FirebaseConfig:ServiceAccountKeyPath" },
};

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
    .WriteTo.Console()
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


// start setting main container
builder.Services.AddControllers();
builder.Services.AddProjectServices(builder.Configuration);


// 加入 Swagger（方便測 API）
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// app build
var app = builder.Build();

// === 在 app 建立完、run 前印出變數
Log.Information("=== Loaded Configuration ===");
foreach (var (envVar, configKey) in envToConfigMap)
{
    var envVal = Environment.GetEnvironmentVariable(envVar) ?? "[null]";
    var configVal = builder.Configuration[configKey] ?? "[null]";
    Log.Information($"{envVar}: {envVal}");
    Log.Information($"{configKey}: {configVal}");
}
Log.Information("=== End Configuration ===");

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
