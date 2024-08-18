using BuildSoft.OscCore;
using OpenShock.VoiceRecognizer.Common;
using OpenShock.VoiceRecognizer.Configuration;

namespace OpenShock.VoiceRecognizer.Integrations.OSC;

public class OSCClient
{
	public OscClient? Client = null;

	public OSCClient()
	{
		GenerateNewClient();
		AttachHandlers();
	}

	private void AttachHandlers()
	{
		ConfigurationState.Instance!.OpenShock.SendHost.ValueChanged += OnSendHostChange;
		ConfigurationState.Instance!.OpenShock.SendPort.ValueChanged += OnSendPortChanged;
	}

	private void OnSendHostChange(object? sender, ValueChangedEventArgs<string> e) =>
		GenerateNewClient();

	private void OnSendPortChanged(object? _, ValueChangedEventArgs<int> e) =>
		GenerateNewClient();

	private void GenerateNewClient() =>
		Client = new(
			ConfigurationState.Instance!.OpenShock.SendHost.Value,
			ConfigurationState.Instance!.OpenShock.SendPort.Value
		);
}
