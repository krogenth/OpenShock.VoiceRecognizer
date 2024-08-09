using Avalonia.Controls;
using Avalonia.Interactivity;
using OpenShock.VoiceRecognizer.Configuration;
using OpenShock.VoiceRecognizer.UI.ViewModels.Settings;

namespace OpenShock.VoiceRecognizer.UI.Views.Settings;

public partial class SettingsZapView : UserControl
{
    public SettingsZapView()
    {
        InitializeComponent();
	}

	private void WordAddClicked(object? sender, RoutedEventArgs e)
	{
		var words = (DataContext as SettingsZapViewModel)!.Words;
		var word = WordBox.Text;

		if (!string.IsNullOrWhiteSpace(word) && !words.Contains(word))
		{
			words.Add(word);
			ConfigurationState.Instance!.Words.Words.Value = (DataContext as SettingsZapViewModel)!.Words;
		}
	}

	private void WordRemoveClicked(object? sender, RoutedEventArgs e)
	{
		var words = (DataContext as SettingsZapViewModel)!.Words;
		var index = (DataContext as SettingsZapViewModel)!.SelectedWordIndex;

		words.RemoveAt(index);

		(DataContext as SettingsZapViewModel)!.SelectedWordIndex = -1;
	}
}
