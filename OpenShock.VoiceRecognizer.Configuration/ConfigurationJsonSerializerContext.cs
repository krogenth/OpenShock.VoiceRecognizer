using System.Text.Json.Serialization;

namespace OpenShock.VoiceRecognizer.Configuration;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(ConfigurationFileFormat))]
internal partial class ConfigurationJsonSerializerContext : JsonSerializerContext
{
}
