using OpenShock.VoiceRecognizer.Configuration;
using OpenShock.VoiceRecognizer.Common;
using OpenShock.VoiceRecognizer.Shockers;

namespace OpenShock.VoiceRecognizer.STT;

public abstract class BaseRecognizer : IDisposable
{
	public event EventHandler<ErrorEventArgs>? ErrorTriggered;
	public event EventHandler<EventArgs>? StateChanged;
	public event EventHandler<RecognizedSpeechEventArgs>? RecognizedSpeech;
	public event EventHandler<WasRecognizedEventArgs>? NGramRecognized;

	public double MSTimestampSinceLastTrigger { get; private set; } = 0.0d;

	protected OpenShockShocker _shocker;

	/// <summary>
	/// Specifies if the Recognizer is currently listening
	/// to the Input device.
	/// </summary>
	public bool Paused { get; protected set; }

	/// <summary>
	/// Specifies if the Recognizer is currently initialized
	/// and can begin listening to the current Input device.
	/// </summary>
	public bool Stopped { get; protected set; }

	public BaseRecognizer()
	{
		_shocker = new OpenShockShocker();
		Paused = false;
		Stopped = true;
		ConfigurationState.Instance!.General.InputDeviceID.ValueChanged += DeviceChanged;
	}

	/// <summary>
	/// Creates the model and begins listening to the selected
	/// Input device for audio samples to be translated.
	/// </summary>
	/// <returns>If the recogizer has started or not</returns>
	public abstract bool Start();
	public bool CanStart => Paused || Stopped;

	/// <summary>
	/// Stops listening to the Input device for audio samples
	/// without removing the underlying model.
	/// </summary>
	public abstract void Pause();
	public bool CanPause => !Paused && !Stopped;

	/// <summary>
	/// Stops listening to the Input device for audio samples
	/// and removes the underlying model.
	/// </summary>
	public abstract void Stop();
	public bool CanStop => !Stopped;

	protected void OnStateChanged() =>
		StateChanged?.Invoke(this, EventArgs.Empty);

	protected void OnRecognizedSpeech(string text)
	{
		RecognizedSpeech?.Invoke(this, new RecognizedSpeechEventArgs(text));
		var recognized = NGramRecognizer.NGramRecognizer.RecognizedNGram(
			text,
			ConfigurationState.Instance!.Shock.Words.Value
		);

		NGramRecognized?.Invoke(this, new WasRecognizedEventArgs(recognized is not null));

		if (recognized is not null)
		{
			// check if the current time has not surpassed the cooldown time for the given recognition
			var currentTime = TimeSpan.FromTicks(DateTime.UtcNow.Ticks).TotalMilliseconds;
			if (currentTime <= MSTimestampSinceLastTrigger + recognized.Cooldown)
			{
				return;
			}

			MSTimestampSinceLastTrigger = currentTime;
			_shocker.HandleRecognizedWord(recognized);
		}
	}

	/// <summary>
	/// Called when an error occurs within the Recognizer,
	/// calls the ErrorTriggered event for listeners to capture.
	/// </summary>
	protected void OnError(Exception e) =>
		ErrorTriggered?.Invoke(this, new ErrorEventArgs(e));

	/// <summary>
	/// Method to attach to ReactiveObjects to detect when a device has changed
	/// from the Configuration State.
	/// We want to avoid having the used Device change mid conversion,
	/// so detect when it changes to stop the Recognizer.
	/// </summary>
	/// <param name="_"></param>
	/// <param name="e"></param>
	protected void DeviceChanged(object? _, ReactiveObject<string>.ValueChangedEventArgs e) =>
		Stop();

	public abstract void Dispose();
}

public class RecognizedSpeechEventArgs(string text)
{
	public string Text { get; } = text;
}

public class WasRecognizedEventArgs(bool value)
{
	public bool Value { get; } = value;
}
