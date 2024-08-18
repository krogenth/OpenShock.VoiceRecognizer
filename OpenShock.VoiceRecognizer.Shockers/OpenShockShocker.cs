using OpenShock.VoiceRecognizer.Configuration;
using OpenShock.VoiceRecognizer.Integrations.OSC;
using OpenShock.VoiceRecognizer.Utility.Common;

namespace OpenShock.VoiceRecognizer.Shockers;

public class OpenShockShocker : BaseShocker
{
	// goes 1 to 10 - anna
	private float _externalSetIntensity = 0.0f;
	private readonly OSCClient _client;

	public OpenShockShocker(OSCServer server) : base(server)
	{
		_client = new();
		AttachHandlers();
	}

	private void AttachHandlers() =>
		_server.OscMessage += OnOscMessageReceived;

	protected override void OnOscMessageReceived(object? sender, OscMessageEventArgs e)
	{
		if (e.Endpoint.Equals(ConfigurationState.Instance!.OpenShock.ExternalListenSetIntensityEndpoint.Value))
		{
			try
			{
				_externalSetIntensity = e.Values.ReadFloatElement(0);
			}
			catch (Exception) { }
		}
	}

	public override async void HandleRecognizedWord(WordRecognition recognized)
	{
		switch (recognized.Type)
		{
			case ShockType.Vibrate:
				HandleVibrate();
				break;
			case ShockType.Shock:
				HandleShock();
				break;
			case ShockType.VibrateThenShock:
				HandleVibrate();
				var delay = (new Random().NextSingle() * (recognized.MaxDelay - recognized.MinDelay)) + recognized.MinDelay;
				await TaskDelay(delay).ConfigureAwait(false);
				HandleShock();
				break;
		}
	}

	private static async Task TaskDelay(float seconds)
	{
		var ms = Convert.ToInt32(Math.Truncate(seconds * 1000));
		await Task.Delay(ms).ConfigureAwait(false);
	}

	protected override void HandleShock()
	{
		_client.Client?.Send(
			$"{ConfigurationState.Instance!.OpenShock.ExternalSendStartShockEndpoint.Value}",
			true
		);
	}

	protected override void HandleVibrate()
	{
		_client.Client?.Send(
			$"{ConfigurationState.Instance!.OpenShock.ExternalSendStartVibrationEndpoint.Value}",
			true
		);
	}

	public override void Dispose()
	{

	}
}
