using System.Text.Json;
using CodeReviews.Console.CodingTracker;
using CodeReviews.Console.CodingTracker.Models;
using Spectre.Console;

string settingsFileName = "appsettings.json";
string jsonSettingsString = File.ReadAllText(settingsFileName);

AppSettings settings = JsonSerializer.Deserialize<AppSettings>(jsonSettingsString)!;

AnsiConsole.MarkupLine(
    $"[blue]Settings set based on configuration file;[/]\nDatabase Path: [green]{settings.Path}[/]\nDatabase Name: [green]{settings.Name}[/]\nConnection String: [green]{settings.ConnectionString}[/]"
);

CodingSession exampleSession = new CodingSession(
    1,
    new DateTime(2025, 12, 22, 12, 0, 0),
    DateTime.Now
);

AnsiConsole.MarkupLine(
    $"[blue]Example CodingSession contains parameters;[/]\nId: [green]{exampleSession.Id}[/]\nStart Time: [green]{exampleSession.StartTime}[/]\nEnd Time: [green]{exampleSession.EndTime}[/]\nDuration: [green]{exampleSession.Duration:c}[/]"
);
