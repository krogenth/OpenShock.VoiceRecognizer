using System.Collections.Generic;
using OpenShock.VoiceRecognizer.Configuration;

namespace OpenShock.VoiceRecognizer.UI.ViewModels.Settings;

public class SettingsZapViewModel : BaseViewModel
{
	public IEnumerable<string> Words { get; set; }

	public SettingsZapViewModel()
	{
		Words = ConfigurationState.Instance!.Words.Words.Value;
	}
}
