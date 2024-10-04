using System.Runtime.Versioning;
using Microsoft.Win32;

namespace OpenShock.VoiceRecognizer.IO;

[SupportedOSPlatform("windows")]
public static class WindowsRegistry
{
	private const string AppPathPath = @"\Software\Microsoft\Windows\CurrentVersion\App Paths\";

	public static string? GetAppPath(string app) =>
		(string?)(Registry.GetValue($"HKEY_LOCAL_MACHINE{AppPathPath}{app}.exe", "", null) ?? null);
}
