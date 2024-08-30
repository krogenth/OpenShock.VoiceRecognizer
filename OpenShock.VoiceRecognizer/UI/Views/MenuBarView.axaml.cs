using Avalonia;
using Avalonia.Controls;
using OpenShock.VoiceRecognizer.UI.Windows;
using OpenShock.VoiceRecognizer.UI.ViewModels;

namespace OpenShock.VoiceRecognizer.UI.Views;

public partial class MenuBarView : UserControl
{
    public MenuBarView()
    {
        InitializeComponent();
	}

	protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
	{
		base.OnAttachedToVisualTree(e);

		if (VisualRoot is MainWindow window)
		{
			DataContext = new MenuBarViewModel(window);
		}
	}
}
