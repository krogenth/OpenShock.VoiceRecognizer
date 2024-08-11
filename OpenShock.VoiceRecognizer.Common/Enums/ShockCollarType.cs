using OpenShock.VoiceRecognizer.Common.Utility;
using System.Text.Json.Serialization;

namespace OpenShock.VoiceRecognizer.Common.Enums;

[JsonConverter(typeof(TypedStringEnumConverter<ShockCollarType>))]
public enum ShockCollarType : int
{
	OpenShock,
	PiShock,
};
