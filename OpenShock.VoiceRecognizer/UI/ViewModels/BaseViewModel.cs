using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OpenShock.VoiceRecognizer.UI.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
