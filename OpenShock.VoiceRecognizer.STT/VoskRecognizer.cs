using System.Text.Json;
using System.Text.Json.Serialization;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using OpenShock.VoiceRecognizer.Configuration;
using OpenShock.VoiceRecognizer.Common.Audio;
using Vosk;
using OpenShock.VoiceRecognizer.Integrations.OSC;

namespace OpenShock.VoiceRecognizer.STT;

public class VoskSpeechRecognizer : BaseRecognizer
{
	private Model? _model;
	private VoskRecognizer? _recognizer;
	private WasapiCapture? _capture;

	public VoskSpeechRecognizer(OSCServer server) : base(server)
	{
		AttachEventHandlers();
	}

	private void AttachEventHandlers() =>
		ConfigurationState.Instance!.Audio.InputDeviceID.ValueChanged += DeviceChanged;

	public override bool Start()
	{
		// check if we are already running, if so, do nothing
		if (!Paused && !Stopped)
		{
			return true;
		}

		string path = ConfigurationState.Instance!.Vosk.ModelDirectory.Value;
		if (string.IsNullOrEmpty(path))
		{
			return false;
		}

		// only if we are not paused do we need to initialize everything
		if (!Paused)
		{
			try
			{
				_model = new Model(path);
				_recognizer = new VoskRecognizer(_model!, 48000f);
			}
			catch (Exception)
			{
				return false;
			}

			var device = AudioDevices.GetDeviceByID(ConfigurationState.Instance.Audio.InputDeviceID.Value);
			_capture = new WasapiCapture(device)
			{
				ShareMode = AudioClientShareMode.Shared,
				WaveFormat = new WaveFormat(48000, 16, 1),
			};
			_capture.DataAvailable += OnDataAvailable;
		}

		// start the recording of device input
		_capture!.StartRecording();

		Paused = false;
		Stopped = false;
		OnStateChanged();
		return true;
	}

	private void OnDataAvailable(object? sender, WaveInEventArgs e)
	{
		// do nothing if we are paused or stopped
		if (Paused || Stopped)
		{
			return;
		}

		if (_recognizer!.AcceptWaveform(e.Buffer, e.BytesRecorded))
		{
			string json = _recognizer.Result();
			var result = JsonSerializer.Deserialize<VoskResult>(json);
			if (result != null && !string.IsNullOrEmpty(result.Text))
			{
				OnRecognizedSpeech(result.Text);
			}
		}
	}

	public override void Pause()
	{
		if (!Paused && !Stopped)
		{
			_capture!.StopRecording();
			Paused = true;
			OnStateChanged();
		}
	}

	public override void Stop()
	{
		if (!Stopped)
		{
			if (_capture != null)
			{
				_capture.StopRecording();
				_capture.DataAvailable -= OnDataAvailable;
				_capture.Dispose();
				_capture = null;
			}

			if (_recognizer != null)
			{
				_recognizer.Dispose();
				_recognizer = null;
			}

			if (_model != null)
			{
				_model.Dispose();
				_model = null;
			}

			Paused = false;
			Stopped = true;
			OnStateChanged();
		}
	}

	public override void Dispose()
	{
		if (!Stopped)
		{
			Stop();
		}
	}
}

public class VoskResultWord
{
	[JsonPropertyName("conf")]
	public float Conf { get; set; }
	[JsonPropertyName("end")]
	public float End { get; set; }
	[JsonPropertyName("start")]
	public float Start { get; set; }
	[JsonPropertyName("word")]
	public string Word { get; set; } = string.Empty;
}

public class VoskResult
{
	[JsonPropertyName("result")]
	public List<VoskResultWord> Result { get; set; } = [];
	[JsonPropertyName("text")]
	public string Text { get; set; } = string.Empty;
}
