using OpenShock.VoiceRecognizer.IO.Json;

namespace OpenShock.VoiceRecognizer.Configuration;

internal static class ConfigurationFileFormatSettings
{
	public static readonly ConfigurationJsonSerializerContext SerializerContext = new(JsonHelper.GetDefaultSerializerOptions());
}
