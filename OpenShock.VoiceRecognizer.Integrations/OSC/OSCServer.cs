using BlobHandles;
using BuildSoft.OscCore;
using OpenShock.VoiceRecognizer.Common;
using OpenShock.VoiceRecognizer.Configuration;

namespace OpenShock.VoiceRecognizer.Integrations.OSC;

public class OSCServer
{
	private OscServer? _server = null;
	public event EventHandler<RecognizerToggleEventArgs>? ToggleRecognizer;
	public event EventHandler<RecognizerSetEventArgs>? SetRecognizer;
	public event EventHandler<OscMessageEventArgs>? OscMessage;

	public OSCServer()
	{
		ConfigurationState.Instance!.OSC.ListenPort.ValueChanged += OnListenPortChanged;
		CreateServer();
	}

	private void CreateServer()
	{
		_server = OscServer.GetOrCreate(ConfigurationState.Instance!.OSC.ListenPort.Value);
		AddServerMethods();

		_server.Start();
	}

	private void AddServerMethods()
	{
		_server!.TryAddMethod("/recognizer/toggle", OnChangeRecognizerState);
		_server!.TryAddMethod("/recognizer/set", OnSetRecognizerState);
		_server!.AddMonitorCallback((BlobString endpoint, OscMessageValues values) =>
			OscMessage?.Invoke(this, new OscMessageEventArgs(endpoint.ToString(), values))
		);
	}

	private void OnListenPortChanged(object? sender, ReactiveObject<int>.ValueChangedEventArgs e) =>
		CreateServer();

	private void OnChangeRecognizerState(OscMessageValues values) =>
		ToggleRecognizer?.Invoke(this, new RecognizerToggleEventArgs());

	private void OnSetRecognizerState(OscMessageValues values)
	{
		try
		{
			SetRecognizer?.Invoke(this, new RecognizerSetEventArgs(values.ReadBooleanElement(0)));
		}
		catch (InvalidOperationException) { }
	}
}

public class RecognizerToggleEventArgs : EventArgs { }

public class RecognizerSetEventArgs(bool state) : EventArgs
{
	public bool State { get; private set; } = state;
}

public class OscMessageEventArgs(string endpoint, OscMessageValues values) : EventArgs
{
	public string Endpoint { get; private set; } = endpoint;
	public OscMessageValues Values { get; private set; } = values;
}
