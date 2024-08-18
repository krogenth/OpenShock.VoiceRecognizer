using OpenShock.VoiceRecognizer.Configuration;
using OpenShock.VoiceRecognizer.Integrations.OSC;

namespace OpenShock.VoiceRecognizer.Shockers;

public abstract class BaseShocker : IDisposable
{
	protected readonly OSCServer _server;

	public BaseShocker(OSCServer server)
	{
		_server = server;
	}

	protected abstract void OnOscMessageReceived(object? sender, OscMessageEventArgs e);

	public abstract void HandleRecognizedWord(WordRecognition recognized);

	protected abstract void HandleVibrate();

	protected abstract void HandleShock();

	public abstract void Dispose();
}
