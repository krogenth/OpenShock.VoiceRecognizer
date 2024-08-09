using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using OpenShock.VoiceRecognizer.UI.Windows;
using OpenShock.VoiceRecognizer.UI.ViewModels;

namespace OpenShock.VoiceRecognizer.UI.Views;

public partial class MenuBarView : UserControl
{
	//public MainWindow Window { get; private set; }

    public MenuBarView()
    {
        InitializeComponent();
	}

	protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
	{
		base.OnAttachedToVisualTree(e);

		if (VisualRoot is MainWindow window)
		{
			//Window = window;
			DataContext = new MenuBarViewModel(window);
		}

	}

	/*public async void OpenSettings(object? sender, RoutedEventArgs e)
	{
		
	}*/
}
