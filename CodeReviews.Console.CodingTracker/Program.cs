using System.Text.Json;
using CodeReviews.Console.CodingTracker;
using Spectre.Console;

string settingsFileName = "appsettings.json";
string jsonSettingsString = File.ReadAllText(settingsFileName);

AppSettings settings = JsonSerializer.Deserialize<AppSettings>(jsonSettingsString)!;

AnsiConsole.MarkupLine(
    $"[blue]Settings set based on configuration file;[/]\nDatabase Path: [green]{settings.Path}[/]\nDatabase Name: [green]{settings.Name}[/]\nConnection String: [green]{settings.ConnectionString}[/]"
);
