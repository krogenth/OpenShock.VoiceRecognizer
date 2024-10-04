using System;
using Avalonia.Controls;
using FluentAvalonia.Core;
using FluentAvalonia.UI.Controls;
using OpenShock.VoiceRecognizer.UI.ViewModels.Settings;

namespace OpenShock.VoiceRecognizer.UI.Windows;

public partial class SettingsWindow : Window
{
	public WordRecognitionWindow? WordRecognitionWindow { get; set; }

	public SettingsWindow()
	{
		var viewModel = new SettingsWindowViewModel(this);
		DataContext = viewModel;
		viewModel.CloseWindow += Close;

		InitializeComponent();
		Load();
	}

	public SettingsWindow(SettingsWindowViewModel viewModel)
	{
		DataContext = viewModel;
		viewModel.CloseWindow += Close;

		InitializeComponent();
		Load();
	}

	private void Load()
	{
		Pages.Children.Clear();
		NavPanel.SelectionChanged += NavPanelOnSelectionChanged;
		NavPanel.SelectedItem = NavPanel.MenuItems.ElementAt(0);
	}

	private void NavPanelOnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
	{
		if (e.SelectedItem is NavigationViewItem navigationViewItem && navigationViewItem.Tag is not null)
		{
			NavPanel.Content = navigationViewItem.Tag.ToString() switch
			{
				"GeneralPage" => GeneralPage,
				"VoskPage" => VoskPage,
				"ZapPage" => ZapPage,
				"BrowserProxyPage" => BrowserProxyPage,
				"OpenShockPage" => OpenShockPage,
				_ => throw new NotImplementedException(),
			};
		}
	}
}
