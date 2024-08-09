using System.Collections.ObjectModel;
using OpenShock.VoiceRecognizer.Configuration;

namespace OpenShock.VoiceRecognizer.UI.ViewModels.Settings;

public class SettingsZapViewModel : BaseViewModel
{
	private int _selectedWordIndex;


	public SettingsZapViewModel()
	{
		Words = ConfigurationState.Instance!.Words.Words.Value;
		_selectedWordIndex = -1;
	}

	public ObservableCollection<string> Words { get; set; }
	public int SelectedWordIndex
	{
		get => _selectedWordIndex;
		set
		{
			_selectedWordIndex = value;
			OnPropertyChanged(nameof(SelectedWordIndex));
			OnPropertyChanged(nameof(HasSelectedWord));
		}
	}
	public bool HasSelectedWord => SelectedWordIndex <= Words.Count && SelectedWordIndex > -1;
}
