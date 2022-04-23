using RandomGameBot;
using RandomGameBot.Core;
using RandomGameBot.Extensions;
using Serilog;
using Spectre.Console;

var prompt = AnsiConsole.Prompt(new SelectionPrompt<string>()
	.Title("Choose what you want to do:")
	.AddChoices("Run the bot", "Check resources", "Enter the editor"));

switch (prompt)
{
	case "Run the bot":
		DoPrerunChecks();
		Startup();
		Bot.Singleton.Start();
		break;
	case "Check resources":
		break;
	case "Enter the editor":
		break;
}

void DoPrerunChecks()
{
	if (!File.Exists("config.json"))
	{
		AnsiConsole.MarkupLine(
			"[yellow bold]Warning! A config.json has not been found and was automatically created. Please, edit its data and run the bot again.[/]");
		Config.Create();
		Environment.Exit(0);
	}

	if (!Directory.Exists("Scripts"))
	{
		AnsiConsole.MarkupLine("[bold red]Scripts/ directory was not found![/]");
		Environment.Exit(1);
	}
	else
	{
		if (!File.Exists("Scripts/GoogleForMe.py"))
		{
			AnsiConsole.MarkupLine("[bold red]Scripts/GoogleForMe.py was not found![/]");
			Environment.Exit(1);
		}
	}

	if (!Directory.Exists("Games"))
		Directory.CreateDirectory("Games");
}

void Startup()
{
	Console.SetOut(ConsoleInterface.RedirectStream);
	
	Log.Logger = new LoggerConfiguration()
		#if DEBUG
		.MinimumLevel.Debug()
		#else
		.MinimumLevel.Information()
		#endif
		.WriteTo.Sink(ConsoleInterface.Sink)
		.CreateLogger();
	ConsoleInterface.Start();
	Log.Debug("Console output redirected");
	Log.Information("Logger initialized");
	
	
	Config.Load();
	Log.Information("Config loaded");
}