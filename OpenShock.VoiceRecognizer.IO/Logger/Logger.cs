using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;

namespace OpenShock.VoiceRecognizer.IO.Logger;

public static class Logger
{
	private static ILoggerFactory? _logFactory;

	public static ILogger<T>? GetLogger<T>() =>
		_logFactory?.CreateLogger<T>();

	public static void Initialize()
	{
		if (_logFactory is not null)
		{
			throw new InvalidOperationException("Log factory already initialize");
		}

		var logConfig = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.WriteTo.File("log.txt");

		Log.Logger = logConfig.CreateLogger();

		_logFactory = new SerilogLoggerFactory(
			Log.Logger
		);
	}
}
