using System.Text.Json.Serialization;
using OpenShock.VoiceRecognizer.Common.Utility;

namespace OpenShock.VoiceRecognizer.Common.Enums;

[JsonConverter(typeof(TypedStringEnumConverter<AudioDeviceType>))]
public enum AudioDeviceType
{
	Input,
	Output,
	Playback,
};
