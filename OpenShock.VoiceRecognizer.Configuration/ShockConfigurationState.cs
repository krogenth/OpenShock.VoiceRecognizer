using System.Collections.ObjectModel;
using OpenShock.VoiceRecognizer.Common;
using OpenShock.VoiceRecognizer.Utility.Common;

namespace OpenShock.VoiceRecognizer.Configuration;

public class ShockConfigurationState
{
	public ReactiveObject<ObservableCollection<WordRecognition>> Words { get; private set; }

	public ShockConfigurationState()
	{
		Words = new([]);
	}

	public void LoadFileConfiguration(ConfigurationFileFormat configurationFileFormat) =>
		Words.Value = configurationFileFormat.Words;

	public void LoadDefaultConfiguration() =>
		Words.Value = [];
}

public class WordRecognition
{
	public string Word { get; set; } = string.Empty;
	public ShockType Type { get; set; }
	public float MinInitialDelay { get; set; }
	public float MaxInitialDelay { get; set; }
	public string InitialDelayRange => $"{MinInitialDelay} - {MaxInitialDelay}";
	public float MinDelay { get; set; }
	public float MaxDelay { get; set; }
	public string DelayRange => $"{MinDelay} - {MaxDelay}";
	public byte MinIntensity { get; set; }
	public byte MaxIntensity { get; set; }
	public string IntensityRange => $"{MinIntensity} - {MaxIntensity}";
	public ushort MinDuration { get; set; }
	public ushort MaxDuration { get; set; }
	public string DurationRange => $"{MinDuration} - {MaxDuration}";
	public double Cooldown { get; set; }
	public string CooldownStr => $"{Cooldown}";
	public bool Active { get; set; }
}
