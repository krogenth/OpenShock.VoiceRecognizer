using System.Linq;
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
		var word = (DataContext as SettingsZapViewModel)!.InputText;
		var shockType = (DataContext as SettingsZapViewModel)!.ShockType;
		var delay = (DataContext as SettingsZapViewModel)!.Delay;

		if (!string.IsNullOrWhiteSpace(word) && !words.Any(w => w.Word.Contains(word)))
		{
			word = word.Trim().ToLower();
			words.Add(new WordRecognition
			{
				Word = word,
				Type = shockType,
				Delay = delay,
			});
			ConfigurationState.Instance!.Shock.Words.Value = words;
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
