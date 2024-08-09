using NAudio.CoreAudioApi;

namespace OpenShock.VoiceRecognizer.Common.Audio;

public static class AudioDevices
{
	public static IEnumerable<MMDevice> GetOutputAudioDevices()
	{
		using var enumerator = new MMDeviceEnumerator();
		return enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
	}

	public static IEnumerable<MMDevice> GetInputAudioDevices()
	{
		using var enumerator = new MMDeviceEnumerator();
		return enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
	}

	public static MMDevice? GetDeviceByID(string id)
	{
		using var mMDeviceEnumerator = new MMDeviceEnumerator();
		return mMDeviceEnumerator.GetDevice(id);
	}
}
