using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace OpenShock.VoiceRecognizer.IO.Json;

public class JsonHelper
{
	private const int DefaultFileWriteBufferSize = 4096;

	public static JsonSerializerOptions GetDefaultSerializerOptions(bool indented = true)
	{
		JsonSerializerOptions options = new()
		{
			DictionaryKeyPolicy = new SnakeCaseNamingPolicy(),
			PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
			WriteIndented = indented,
			AllowTrailingCommas = true,
			ReadCommentHandling = JsonCommentHandling.Skip,
		};

		return options;
	}

	public static string Serialize<T>(T value, JsonTypeInfo<T> typeInfo) => JsonSerializer.Serialize(value, typeInfo);
	public static T? Deserialize<T>(string value, JsonTypeInfo<T> typeInfo) => JsonSerializer.Deserialize(value, typeInfo);

	public static void SerializeToFile<T>(string filepath, T value, JsonTypeInfo<T> typeInfo)
	{
		using FileStream file = File.Create(filepath, DefaultFileWriteBufferSize, FileOptions.WriteThrough);
		JsonSerializer.Serialize(file, value, typeInfo);
	}

	public static T? DeserializeFromFile<T>(string filepath, JsonTypeInfo<T> typeInfo)
	{
		using FileStream file = File.OpenRead(filepath);
		return JsonSerializer.Deserialize<T>(file, typeInfo);
	}

	private class SnakeCaseNamingPolicy : JsonNamingPolicy
	{
		public override string ConvertName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return name;
			}

			StringBuilder builder = new();

			for (int i = 0; i < name.Length; i++)
			{
				char c = name[i];

				if (char.IsUpper(c))
				{
					if (i == 0 || char.IsUpper(name[i - 1]))
					{
						builder.Append(char.ToLowerInvariant(c));
					}
					else
					{
						builder.Append('_');
						builder.Append(char.ToLowerInvariant(c));
					}
				}
				else
				{
					builder.Append(c);
				}
			}

			return builder.ToString();
		}
	}
}
