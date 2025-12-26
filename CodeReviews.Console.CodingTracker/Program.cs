using CodeReviews.Console.CodingTracker;
using Microsoft.Extensions.Configuration;

try
{
    Init();
}
catch (Exception ex)
{
    Console.WriteLine($"There was an error during application startup: {ex.Message}");
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

    CodingTracker codingTracker = new(connectionString);
}
