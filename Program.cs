
using RestApiPractice.Extensions;

using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;

using Serilog;
using Serilog.Events;

var port = Environment.GetEnvironmentVariable("PORT") ?? "80";
var builder = WebApplication.CreateBuilder(args);

// log
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

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port));
});






var configuration = builder.Configuration;
builder.Services.AddProjectServices(builder.Configuration);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// 加入 Controller
builder.Services.AddControllers();
// 加入 Swagger（方便測 API）
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// 開發環境用 Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll"); 

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
