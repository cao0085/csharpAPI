using RestApiPractice.DataLayer.Repositories;
using RestApiPractice.LogicLayer;
using RestApiPractice.LogicLayer.Interfaces;

using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

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

var configuration = builder.Configuration;
// 註冊 DI (邏輯層、資料層)
builder.Services.AddScoped<IProductService, ProductSelectionLogic>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();



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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
