using Avalonia.Controls;
using OpenShock.VoiceRecognizer.UI.ViewModels;

namespace OpenShock.VoiceRecognizer.UI.Windows;

public partial class WordRecognitionWindow : Window
{
	public WordRecognitionWindow(WordRecognitionWindowViewModel viewModel)
	{
		DataContext = viewModel;
		viewModel.CloseWindow += Close;

		InitializeComponent();
	}
}
