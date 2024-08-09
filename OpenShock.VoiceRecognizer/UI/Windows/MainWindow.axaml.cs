using Avalonia.Controls;

namespace OpenShock.VoiceRecognizer.UI.Windows;

public partial class MainWindow : Window
{
	public SettingsWindow? SettingsWindow { get; set; }

	public MainWindow()
    {
        InitializeComponent();
    }
}
