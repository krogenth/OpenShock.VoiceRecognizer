using OpenShock.VoiceRecognizer.Common;
using OpenShock.VoiceRecognizer.Common.Enums;
using OpenShock.VoiceRecognizer.Configuration;
using OpenShock.VoiceRecognizer.Integrations.OSC;
using OpenShock.VoiceRecognizer.STT;
using OpenShock.VoiceRecognizer.STT.BrowserProxy;
using System;

namespace OpenShock.VoiceRecognizer.UI.ViewModels;

public class MainModelViewModel : BaseViewModel
{
	private readonly OSCServer _oscServer;
	private string _recognizedText = string.Empty;
	private bool _wasRecognized = false;

	public BaseRecognizer? BaseSpeechRecognizer { get; private set; }

	public string RecognizedText
	{
		get => _recognizedText;
		set
		{
			_recognizedText = value ?? string.Empty;
			OnPropertyChanged(nameof(RecognizedText));
		}
	}

	public bool WasRecognized
	{
		get => _wasRecognized;
		set
		{
			_wasRecognized = value;
			OnPropertyChanged(nameof(WasRecognized));
		}
	}

	public MainModelViewModel()
	{
		_oscServer = new OSCServer();
		_oscServer.ToggleRecognizer += OnOSCRecognizerToggle;
		_oscServer.SetRecognizer += OnOSCRecognizerSet;

		SelectRecognizer();
		ConfigurationState.Instance!.General.Recognizer.ValueChanged += OnRecognizerChanged;
		ConfigurationState.Instance!.BrowserProxy.BrowserProxy.ValueChanged += OnBrowserProxyChanged;
	}

	private void SelectRecognizer()
	{
		if (BaseSpeechRecognizer is not null)
		{
			if (BaseSpeechRecognizer.CanStop)
			{
				BaseSpeechRecognizer.Stop();
			}

			BaseSpeechRecognizer = null;
		}

		switch (ConfigurationState.Instance!.General.Recognizer.Value)
		{
		case RecognizerType.Vosk:
			BaseSpeechRecognizer = new VoskSpeechRecognizer();
			break;
		case RecognizerType.BrowserProxy:
			switch (ConfigurationState.Instance!.BrowserProxy.BrowserProxy.Value)
			{
			case BrowserProxyType.Chrome:
				BaseSpeechRecognizer = new ChromeBrowserProxyRecognizer();
				break;
			case BrowserProxyType.Edge:
				BaseSpeechRecognizer = new EdgeBrowserProxyRecognizer();
				break;
			}

			break;
		}

		if (BaseSpeechRecognizer is not null)
		{
			BaseSpeechRecognizer.RecognizedSpeech += OnRecognizedSpeech;
			BaseSpeechRecognizer.NGramRecognized += WasRecognizedSpeech;
			BaseSpeechRecognizer.StateChanged += OnRecognizerStateChanged;
		}
	}

	private void OnRecognizerChanged(object? sender, ReactiveObject<RecognizerType>.ValueChangedEventArgs _) =>
		SelectRecognizer();

	private void OnBrowserProxyChanged(object? sender, ReactiveObject<BrowserProxyType>.ValueChangedEventArgs _)
	{
		if (ConfigurationState.Instance!.General.Recognizer.Value is RecognizerType.BrowserProxy)
		{
			SelectRecognizer();
		}
	}

	private void OnRecognizedSpeech(object? sender, RecognizedSpeechEventArgs e) =>
		RecognizedText = e.Text;

	private void WasRecognizedSpeech(object? sender, WasRecognizedEventArgs e) =>
		WasRecognized = e.Value;

	private void OnOSCRecognizerToggle(object? sender, RecognizerToggleEventArgs e)
	{
		if (BaseSpeechRecognizer == null)
		{
			return;
		}

		if (BaseSpeechRecognizer!.CanStart)
		{
			BaseSpeechRecognizer!.Start();
		}
		else
		{
			BaseSpeechRecognizer!.Stop();
		}
	}

	private void OnOSCRecognizerSet(object? sender, RecognizerSetEventArgs e)
	{
		if (e.State == false && CanStopRecognizer)
		{
			BaseSpeechRecognizer!.Stop();
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
