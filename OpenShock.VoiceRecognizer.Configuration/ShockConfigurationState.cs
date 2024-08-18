using System.Collections.ObjectModel;
using OpenShock.VoiceRecognizer.Common;
using OpenShock.VoiceRecognizer.Common.Enums;
using OpenShock.VoiceRecognizer.Utility.Common;

namespace OpenShock.VoiceRecognizer.Configuration;

public class ShockConfigurationState
{
	public ReactiveObject<ShockCollarType> CollarType { get; private set; }
	public ReactiveObject<ObservableCollection<WordRecognition>> Words { get; private set; }

	public ShockConfigurationState()
	{
		CollarType = new(ShockCollarType.OpenShock);
		Words = new([]);
	}

	public void LoadFileConfiguration(ConfigurationFileFormat configurationFileFormat)
	{
		CollarType.Value = configurationFileFormat.CollarType;
		Words.Value = configurationFileFormat.Words;
	}

	public void LoadDefaultConfiguration()
	{
		CollarType.Value = ShockCollarType.OpenShock;
		Words.Value = [];
	}
}

public class WordRecognition
{
	public string Word { get; set; } = string.Empty;
	public ShockType Type { get; set; }
	public float MinDelay { get; set; }
	public float MaxDelay { get; set; }
}
