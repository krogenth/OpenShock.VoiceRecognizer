using System.Reactive;
using ReactiveUI;
using OpenShock.VoiceRecognizer.UI.Windows;

namespace OpenShock.VoiceRecognizer.UI.ViewModels;

public class MenuBarViewModel : BaseViewModel
{
	public MainWindow Window { get; set; }

	public MenuBarViewModel(MainWindow window)
	{
		Window = window;
		OpenSettingsWindowCommand = ReactiveCommand.Create(OpenSettingsWindow);
	}

	public ReactiveCommand<Unit, Unit> OpenSettingsWindowCommand { get; }

	public async void OpenSettingsWindow()
	{
		Window.SettingsWindow = new();
		await Window.SettingsWindow.ShowDialog(Window).ConfigureAwait(false);
		Window.SettingsWindow = null;
	}
}
