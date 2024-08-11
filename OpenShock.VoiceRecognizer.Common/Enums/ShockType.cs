using OpenShock.VoiceRecognizer.Common.Utility;
using System.Text.Json.Serialization;

namespace OpenShock.VoiceRecognizer.Utility.Common;

[JsonConverter(typeof(TypedStringEnumConverter<ShockType>))]
public enum ShockType : int
{
	Vibrate,
	Shock,
	VibrateThenShock,
};
