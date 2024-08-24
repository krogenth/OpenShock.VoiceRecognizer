using OpenShock.VoiceRecognizer.Configuration;
using OpenShock.VoiceRecognizer.Integrations.OSC;

namespace OpenShock.VoiceRecognizer.Shockers;

public abstract class BaseShocker : IDisposable
{
	public BaseShocker()
	{
		
	}

	public abstract void HandleRecognizedWord(WordRecognition recognized);

	protected abstract Task HandleVibrate(Guid deviceID, ushort duration, byte intensity);

	protected abstract Task HandleShock(Guid deviceID, ushort duration, byte intensity);

	public abstract void Dispose();
}
