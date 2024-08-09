﻿using System.Reactive;
using ReactiveUI;
using OpenShock.VoiceRecognizer.UI.Windows;

namespace OpenShock.VoiceRecognizer.UI.ViewModels;

public class MenuBarViewModel : BaseViewModel
{
	public MainWindow Window { get; set; }

	public MenuBarViewModel(MainWindow window)
	{
		Window = window;
		OpenSettingsCommand = ReactiveCommand.Create(OpenSettings);
	}

	public ReactiveCommand<Unit, Unit> OpenSettingsCommand { get; }

	public async void OpenSettings()
	{
		Window!.SettingsWindow = new(new Settings.SettingsWindowViewModel());
		await Window.SettingsWindow.ShowDialog(Window).ConfigureAwait(false);
		Window.SettingsWindow = null;
	}
}