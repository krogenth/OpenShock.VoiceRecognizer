using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenShock.VoiceRecognizer.Common.Utility;

public sealed class TypedStringEnumConverter<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum
{
	public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var enumValue = reader.GetString();

		if (Enum.TryParse(enumValue, out TEnum value))
		{
			return value;
		}

		return default;
	}

	public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options) =>
		writer.WriteStringValue(value.ToString());
}
