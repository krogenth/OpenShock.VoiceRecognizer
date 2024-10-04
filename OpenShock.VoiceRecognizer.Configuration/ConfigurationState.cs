namespace OpenShock.VoiceRecognizer.Configuration;

public class ConfigurationState
{
	private readonly string _fileLocation = $"{AppDomain.CurrentDomain.BaseDirectory}config.json";

	public GeneralConfigurationState General { get; }
	public VoskConfigurationState Vosk { get; }
	public OSCConfigurationState OSC { get; }
	public ShockConfigurationState Shock { get; }
	public OpenShockConfigurationState OpenShock { get; }
	public BrowserProxyConfigurationState BrowserProxy { get; }

	public static ConfigurationState? Instance { get; private set; } = null;

	private ConfigurationState()
	{
		General = new();
		Vosk = new();
		OSC = new();
		Shock = new();
		OpenShock = new();
		BrowserProxy = new();
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
			General.LoadFileConfiguration(configurationFileFormat!);
			Vosk.LoadFileConfiguration(configurationFileFormat!);
			OSC.LoadFileConfiguration(configurationFileFormat!);
			Shock.LoadFileConfiguration(configurationFileFormat!);
			OpenShock.LoadFileConfiguration(configurationFileFormat!);
			BrowserProxy.LoadFileConfiguration(configurationFileFormat!);
		}
	}

	private void LoadDefaultConfigurationState()
	{
		General.LoadDefaultConfiguration();
		Vosk.LoadDefaultConfiguration();
		OSC.LoadDefaultConfiguration();
		Shock.LoadDefaultConfiguration();
		OpenShock.LoadDefaultConfiguration();
		BrowserProxy.LoadDefaultConfiguration();
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
