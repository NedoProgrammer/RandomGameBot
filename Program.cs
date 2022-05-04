using RandomGameBot;
using RandomGameBot.Core;
using RandomGameBot.Features.ObjectionLol;
using Serilog;
using Spectre.Console;

namespace RandomGameBot;

class Program
{
	#region Main Menu

	/// <summary>
	/// Entry point of the bot.
	/// </summary>
	public static void Main()
	{
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
	}
	#endregion
	
	#region Bot Startup

	/// <summary>
	/// Checks for required resources which are necessary for running the bot.
	/// </summary>
	static void DoPrerunChecks()
    {
    	if (!File.Exists("config.json"))
    	{
    		AnsiConsole.MarkupLine(
    			"[yellow bold]Warning! A config.json has not been found and was automatically created. Please, edit its data and run the bot again.[/]");
    		Config.Create();
    		Environment.Exit(0);
    	}
    	
    	CheckResource("Scripts", true);
    	CheckResource("Scripts/GoogleForMe.py");
    	
    	CheckResource("Data", true);
    	CheckResource("Data/GoogleForMe.txt", false, false);
    	CheckResource("Data/Cheatsheet.png");
    	CheckResource("Data/Preferences.json", false, false, "{}");
    	
    	CheckResource("Games", true, false);
    	CheckResource("Games/GoogleForMe", true, false);
    }

	/// <summary>
	/// Check if a certain app resource exists.
	/// </summary>
	/// <param name="name">Name of the resource</param>
	/// <param name="directory">Is it a directory?</param>
	/// <param name="fatal">If it doesn't exist, should the app exit?</param>
	/// <param name="fileContent">If a file does not exist, but it's also not fatal, this will be written into it.</param>
	static void CheckResource(string name, bool directory = false, bool fatal = true, string fileContent = "")
    {
    	if (directory && !Directory.Exists(name))
    	{
    		if (fatal)
    		{
    			AnsiConsole.MarkupLine($"[bold red]${name}/ directory was not found![/]");
    			Environment.Exit(1);
    		}
    		else Directory.CreateDirectory(name);
    	}
    	else if (!File.Exists(name))
    	{
    		if (fatal)
    		{
    			AnsiConsole.MarkupLine($"[bold red]${name} file was not found![/]");
    			Environment.Exit(1);
    		}
    		else File.WriteAllText(name, fileContent);
    	}
    }
	/// <summary>
	/// Start the bot. (Redirect streams, initialize loggers, etc.)
	/// </summary>
    static void Startup()
    {
	    //Redirect all Serilog and Console output to the RedirectStream
    	Console.SetOut(ConsoleInterface.RedirectStream);
    
    	Log.Logger = new LoggerConfiguration()
    		#if DEBUG
    		.MinimumLevel.Debug()
    		#else
    		.MinimumLevel.Information()
    		#endif
            //Write to a custom sink (which is actually a regular console sink)
    		.WriteTo.Sink(ConsoleInterface.Sink)
    		.CreateLogger();
        
        ConsoleInterface.Start();
    	Log.Debug("Console output redirected");
    	Log.Information("Logger initialized");
    
    
    	Config.Load();
    	Log.Information("Config loaded");
    
        //Load features
    	ObjectionLolCommands.Initialize();
    	CharacterPreferences.Load();
    }
    #endregion
}
