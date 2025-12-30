using CodeReviews.Console.CodingTracker.aneevel;
using Microsoft.Extensions.Configuration;
using Serilog;

try
{
    Init();
    Shutdown();
}
catch (Exception ex)
{
    Console.WriteLine($"There was an error during application execution: {ex.Message}");
}

void Init()
{
    IConfiguration systemConfiguration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    var connectionString =
        systemConfiguration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException(
            "Unable to find default connection string; exiting..."
        );

    var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
    Directory.CreateDirectory(logDirectory);
    var logFilePath = Path.Combine(logDirectory, "log_.txt");
    var outputTemplate =
        "[{Timestamp:yyyy-MM-dd HH:mm:ss} ({Level:u3})] {Message:lj}{NewLine}{Exception}";

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.Console()
        .WriteTo.File(
            path: logFilePath,
            outputTemplate: outputTemplate,
            rollingInterval: RollingInterval.Day
        )
        .CreateLogger();

    CodingTracker codingTracker = new(connectionString);
}

void Shutdown()
{
    Log.CloseAndFlush();
}
