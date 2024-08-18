using OpenShock.VoiceRecognizer.Integrations.OSC;
using OpenShock.VoiceRecognizer.STT;
using System;

namespace OpenShock.VoiceRecognizer.UI.ViewModels;

public class MainModelViewModel : BaseViewModel
{
	private readonly OSCServer _oscServer;
	private string _recognizedText = string.Empty;

	public BaseRecognizer? BaseSpeechRecognizer { get; set; }

	public string RecognizedText
	{
		get => _recognizedText;
		set
		{
			_recognizedText = value ?? string.Empty;
			OnPropertyChanged(nameof(RecognizedText));
		}
	}

	public MainModelViewModel()
	{
		_oscServer = new OSCServer();
		_oscServer.ToggleRecognizer += OnOSCRecognizerToggle;
		_oscServer.SetRecognizer += OnOSCRecognizerSet;

		SelectRecognizer();
	}

	private void SelectRecognizer()
	{
		BaseSpeechRecognizer = new VoskSpeechRecognizer(_oscServer);
		BaseSpeechRecognizer.RecognizedSpeech += OnRecognizedSpeech;
		BaseSpeechRecognizer.StateChanged += OnRecognizerStateChanged;
	}

	private void OnRecognizedSpeech(object? sender, RecognizedSpeechEventArgs e) =>
		RecognizedText = e.Text;

	private void OnOSCRecognizerToggle(object? sender, RecognizerToggleEventArgs e)
	{
		if (BaseSpeechRecognizer == null)
		{
			return;
		}

		if (BaseSpeechRecognizer!.Paused)
		{
			BaseSpeechRecognizer!.Start();
		}
		else
		{
			BaseSpeechRecognizer!.Pause();
		}
	}

	private void OnOSCRecognizerSet(object? sender, RecognizerSetEventArgs e)
	{
		if (e.State == false && CanPauseRecognizer)
		{
			BaseSpeechRecognizer!.Pause();
		}
		else if (e.State == true && CanStartRecognizer)
		{
			BaseSpeechRecognizer!.Start();
		}
	}

	// Speech Recognizer methods & properties
	public bool CanStartRecognizer => BaseSpeechRecognizer!.CanStart;
	public bool StartRecognizer() => BaseSpeechRecognizer!.Start();

	public bool CanPauseRecognizer => BaseSpeechRecognizer!.CanPause;
	public void PauseRecognizer(object _) => BaseSpeechRecognizer!.Pause();

	public bool CanStopRecognizer => BaseSpeechRecognizer!.CanStop;
	public void StopRecognizer(object _) => BaseSpeechRecognizer!.Stop();

	private void NotifyRecognizerChanged()
	{
		OnPropertyChanged(nameof(CanStartRecognizer));
		OnPropertyChanged(nameof(CanPauseRecognizer));
		OnPropertyChanged(nameof(CanStopRecognizer));
	}

	protected void OnRecognizerStateChanged(object? sender, EventArgs _) =>
		NotifyRecognizerChanged();
}
