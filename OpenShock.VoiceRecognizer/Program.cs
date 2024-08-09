using Avalonia;
using Avalonia.ReactiveUI;
using System;
using OpenShock.VoiceRecognizer.Configuration;

namespace OpenShock.VoiceRecognizer;

internal sealed class Program
{
	// Initialization code. Don't use any Avalonia, third-party APIs or any
	// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
	// yet and stuff might break.
	[STAThread]
	public static void Main(string[] args)
	{
		InitializeConfig();

		BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
	}

	// Avalonia configuration, don't remove; also used by visual designer.
	public static AppBuilder BuildAvaloniaApp()
	{
		return AppBuilder.Configure<App>()
			.UsePlatformDetect()
			.WithInterFont()
			.LogToTrace()
			.UseReactiveUI();
	}

	public static void InitializeConfig()
	{
		ConfigurationState.Initialize();
		ConfigurationState.Instance!.LoadConfiguration();
	}
}
