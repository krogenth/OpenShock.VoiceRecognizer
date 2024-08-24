using OpenShock.VoiceRecognizer.Configuration;
using OpenShock.VoiceRecognizer.Integrations.OpenShock;
using OpenShock.VoiceRecognizer.Integrations.OSC;
using OpenShock.VoiceRecognizer.Utility.Common;

namespace OpenShock.VoiceRecognizer.Shockers;

public class OpenShockShocker : BaseShocker
{
	public OpenShockShocker()
	{
	}

	public override async void HandleRecognizedWord(WordRecognition recognized)
	{
		var deviceID = ConfigurationState.Instance!.OpenShock.ShockerID.Value;
		var initialDelay = (new Random().NextSingle() * (recognized.MaxInitialDelay - recognized.MinInitialDelay)) + recognized.MinInitialDelay;
		await TaskDelay(initialDelay).ConfigureAwait(false);

		var duration = (ushort)((new Random().Next() * (recognized.MaxDuration - recognized.MinDuration)) + recognized.MinDuration);

		switch (recognized.Type)
		{
			case ShockType.Vibrate:
				await HandleVibrate(deviceID, duration, recognized.Intensity).ConfigureAwait(false);
				break;
			case ShockType.Shock:
				await HandleShock(deviceID, duration, recognized.Intensity).ConfigureAwait(false);
				break;
			case ShockType.VibrateThenShock:
				await HandleVibrate(deviceID, duration, recognized.Intensity).ConfigureAwait(false);
				var delay = (new Random().NextSingle() * (recognized.MaxDelay - recognized.MinDelay)) + recognized.MinDelay;
				await TaskDelay(delay);
				await HandleShock(deviceID, duration, recognized.Intensity).ConfigureAwait(false);
				break;
		}
	}

	private static async Task TaskDelay(float seconds)
	{
		var ms = Convert.ToInt32(Math.Truncate(seconds * 1000));
		await Task.Delay(ms).ConfigureAwait(false);
	}

	protected override async Task HandleShock(Guid deviceID, ushort duration, byte intensity)
	{
		await OpenShockAPI.Instance.SendShock(deviceID, duration, intensity);
	}

	protected override async Task HandleVibrate(Guid deviceID, ushort duration, byte intensity)
	{
		await OpenShockAPI.Instance.SendVibrate(deviceID, duration, intensity);
	}

	public override void Dispose()
	{

	}
}
