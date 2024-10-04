using System;
using Avalonia.Controls;

namespace OpenShock.VoiceRecognizer.UI.Windows;

public partial class MainWindow : Window
{
	public SettingsWindow? SettingsWindow { get; set; }

	public MainWindow()
    {
        InitializeComponent();
    }

	protected override void OnClosing(WindowClosingEventArgs e)
	{
		// this feels dirty, can I make this better...?
		Environment.Exit(Environment.ExitCode);
		base.OnClosing(e);
	}
}
