using System.Diagnostics;
using OpenShock.VoiceRecognizer.Configuration;
using OpenShock.VoiceRecognizer.IO;

namespace OpenShock.VoiceRecognizer.STT.BrowserProxy;

public sealed class ChromeBrowserProxyRecognizer : BaseBrowserProxyRecognizer
{
	public ChromeBrowserProxyRecognizer() { }

	public override void Pause()
	{
		// need to communicate with thread to not process any more data...
		Paused = true;
		Stopped = false;
		OnStateChanged();
	}

	public override bool Start()
	{
		StartProxy(ConfigurationState.Instance!.BrowserProxy.ProxyPort.Value);
		OpenBrowser(ConfigurationState.Instance!.BrowserProxy.ProxyPort.Value);

		Paused = false;
		Stopped = false;
		OnStateChanged();
		return true;
	}

	public override void Stop()
	{
		StopProxy();
		Paused = false;
		Stopped = true;
		OnStateChanged();
	}

	public override void Dispose() => Stop();

	protected override void OpenBrowser(int port)
	{
		string? browserPath = string.Empty;

		if (OperatingSystem.IsWindows())
		{
			browserPath = WindowsRegistry.GetAppPath("chrome");
		}
		else if (OperatingSystem.IsLinux())
		{
			browserPath = "google-chrome";
		}
		else if (OperatingSystem.IsMacOS())
		{
			browserPath = "/Applications/Google Chrome.app/Contents/MacOS/Google Chrome";
		}

		if (browserPath is not null)
		{
			Process.Start(browserPath, $"http://localhost:{port}");
		}
	}

	protected override void DetectBrowser()
	{
		// what should this even do...?
	}

	protected override void CloseBrowser()
	{
		// how do we actually close the browser tab...?
		// need to send a javascript command for it to close itself...
	}
}
