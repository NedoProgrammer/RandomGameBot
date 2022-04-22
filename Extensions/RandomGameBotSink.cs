using Serilog.Core;
using Serilog.Events;

namespace RandomGameBot.Extensions;

public class RandomGameBotSink: ILogEventSink
{
	private readonly IFormatProvider _formatProvider;

	public RandomGameBotSink(IFormatProvider formatProvider)
	{
		_formatProvider = formatProvider;
		File.WriteAllText("latest_log.txt", "");
	}

	public event Action? Emitted;
	
	public void Emit(LogEvent logEvent)
	{
		var message = logEvent.RenderMessage(_formatProvider);
		var formatted = $"{DateTime.Now:g}:{DateTime.Now.Millisecond} [{logEvent.Level.ToString()}] {message}";
		#pragma warning disable Spectre1000
		Console.WriteLine(formatted);
		File.AppendAllText("latest_log.txt", formatted + "\n");
		#pragma warning restore Spectre1000
		Emitted?.Invoke();
	}
}