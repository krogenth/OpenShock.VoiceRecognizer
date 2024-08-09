using System.Collections.ObjectModel;
using OpenShock.VoiceRecognizer.Common;

namespace OpenShock.VoiceRecognizer.Configuration;

public class WordsConfigurationState
{
	public ReactiveObject<ObservableCollection<string>> Words { get; private set; }

	public WordsConfigurationState()
	{
		Words = new([]);
	}

	public void LoadFileConfiguration(ConfigurationFileFormat configurationFileFormat) =>
		Words.Value = configurationFileFormat.Words;

	public void LoadDefaultConfiguration() =>
		Words = new([]);
}
