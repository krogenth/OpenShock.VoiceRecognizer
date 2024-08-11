using OpenShock.VoiceRecognizer.Common.Enums;

namespace OpenShock.VoiceRecognizer.Configuration;

public class ConfigurationState
{
	private readonly string _fileLocation = $"{AppDomain.CurrentDomain.BaseDirectory}config.json";

	public AudioConfigurationState Audio { get; }
	public VoskConfigurationState Vosk { get; }
	public OSCConfigurationState OSC { get; }
	public ShockConfigurationState Shock { get; }

	public static ConfigurationState? Instance { get; private set; } = null;

	private ConfigurationState()
	{
		Audio = new();
		Vosk = new();
		OSC = new();
		Shock = new();
	}

	public void LoadConfiguration()
	{
		if (File.Exists(_fileLocation))
		{
			LoadConfigurationStateFromFile();
		}
		else
		{
			LoadDefaultConfigurationState();
		}
	}

	private void LoadConfigurationStateFromFile()
	{
		if (ConfigurationFileFormat.TryLoad(_fileLocation, out ConfigurationFileFormat? configurationFileFormat))
		{
			Audio.LoadFileConfiguration(configurationFileFormat!);
			Vosk.LoadFileConfiguration(configurationFileFormat!);
			OSC.LoadFileConfiguration(configurationFileFormat!);
			Shock.LoadFileConfiguration(configurationFileFormat!);
		}
	}

	private void LoadDefaultConfigurationState()
	{
		Audio.LoadDefaultConfiguration();
		Vosk.LoadDefaultConfiguration();
		OSC.LoadDefaultConfiguration();
		Shock.LoadDefaultConfiguration();
	}

	public void SaveConfigurationStateToFile() =>
		new ConfigurationFileFormat(this).SaveFile(_fileLocation);

	public static void Initialize()
	{
		if (Instance != null)
		{
			throw new InvalidOperationException("Configuration is already initialized");
		}

		Instance = new();
	}
}
