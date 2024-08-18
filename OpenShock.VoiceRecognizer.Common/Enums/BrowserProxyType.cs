using OpenShock.VoiceRecognizer.Common.Utility;
using System.Text.Json.Serialization;

namespace OpenShock.VoiceRecognizer.Common.Enums;

[JsonConverter(typeof(TypedStringEnumConverter<BrowserProxyType>))]
public enum BrowserProxyType
{
	Chrome,
	Edge,
	Opera,
	Firefox,
};
