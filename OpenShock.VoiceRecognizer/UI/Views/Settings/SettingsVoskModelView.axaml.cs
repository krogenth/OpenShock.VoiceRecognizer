using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.VisualTree;
using OpenShock.VoiceRecognizer.UI.ViewModels.Settings;

namespace OpenShock.VoiceRecognizer.UI.Views.Settings;

public partial class SettingsVoskModelView : UserControl
{
    public SettingsVoskModelView()
    {
        InitializeComponent();
    }

	private async void ChangeModelDirectory_OnClick(object sender, RoutedEventArgs e)
	{
		var dataContext = DataContext as SettingsVoskModelViewModel;
		if (this.GetVisualRoot() is Window window)
		{
			var storagefolder = await window.StorageProvider.TryGetFolderFromPathAsync(
				dataContext!.SelectedModelDirectory
			).ConfigureAwait(true);
			var result = await window.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
			{
				AllowMultiple = false,
				SuggestedStartLocation = storagefolder,
			}).ConfigureAwait(true);

			if (result != null && result.Count > 0)
			{
				dataContext!.SelectedModelDirectory = result[0].Path.LocalPath;
			}
		}
	}
}
