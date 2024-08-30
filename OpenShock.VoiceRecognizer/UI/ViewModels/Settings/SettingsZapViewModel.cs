using System.Collections.ObjectModel;
using System.Reactive;
using OpenShock.VoiceRecognizer.Configuration;
using ReactiveUI;
using OpenShock.VoiceRecognizer.UI.Windows;
using System.Linq;

namespace OpenShock.VoiceRecognizer.UI.ViewModels.Settings;

public class SettingsZapViewModel : BasedSettingsViewModel
{
	public SettingsWindow Window { get; set; }
	private WordRecognitionWindowViewModel _wordRecognitionWindowVM;
	private bool _isEditWordRecognition = false;

	private int _selectedWordIndex;
	public ObservableCollection<WordRecognition> Words { get; set; }

	public SettingsZapViewModel(SettingsWindow window)
	{
		_selectedWordIndex = -1;
		Words = new(ConfigurationState.Instance!.Shock.Words.Value);

		Window = window;
		OpenAddWordRecognitionWindowCommand = ReactiveCommand.Create(OpenAddWordRecognitionWindow);
		OpenEditWordRecognitionWindowCommand = ReactiveCommand.Create(OpenEditWordRecognitionWindow);
		RemoveWordCommand = ReactiveCommand.Create(RemoveWord);
	}

	public ReactiveCommand<Unit, Unit> OpenAddWordRecognitionWindowCommand { get; }

	public async void OpenAddWordRecognitionWindow()
	{
		if (Window is not null)
		{
			_isEditWordRecognition = false;
			_wordRecognitionWindowVM = new();
			_wordRecognitionWindowVM.SaveWord += OnWordRecognitionSave;
			Window.WordRecognitionWindow = new(_wordRecognitionWindowVM);
			await Window.WordRecognitionWindow.ShowDialog(Window).ConfigureAwait(false);
			Window.WordRecognitionWindow = null;
		}
	}

	public ReactiveCommand<Unit, Unit> OpenEditWordRecognitionWindowCommand { get; }

	public async void OpenEditWordRecognitionWindow()
	{
		if (Window is not null)
		{
			_isEditWordRecognition = true;
			_wordRecognitionWindowVM = new(Words[SelectedWordIndex]);
			_wordRecognitionWindowVM.SaveWord += OnWordRecognitionSave;
			Window.WordRecognitionWindow = new(_wordRecognitionWindowVM);
			await Window.WordRecognitionWindow.ShowDialog(Window).ConfigureAwait(false);
			Window.WordRecognitionWindow = null;
		}
	}

	public ReactiveCommand<Unit, Unit> RemoveWordCommand { get; }

	public void RemoveWord()
	{
		Words.RemoveAt(SelectedWordIndex);
		SelectedWordIndex = -1;
	}

	public override void SaveToConfigurationState() =>
		ConfigurationState.Instance!.Shock.Words.Value = Words;

	private void OnWordRecognitionSave()
	{
		if (!string.IsNullOrWhiteSpace(_wordRecognitionWindowVM.InputText) &&
			!Words.ToList().Any(w =>w.Word.Contains(_wordRecognitionWindowVM.InputText, System.StringComparison.CurrentCultureIgnoreCase))
		)
		{
			if (!_isEditWordRecognition)
			{
				Words.Add(new WordRecognition()
				{
					Word = _wordRecognitionWindowVM.InputText,
					Type = _wordRecognitionWindowVM.ShockType,
					MinInitialDelay = _wordRecognitionWindowVM.MinInitialDelay,
					MaxInitialDelay = _wordRecognitionWindowVM.MaxInitialDelay,
					MinDelay = _wordRecognitionWindowVM.MinDelay,
					MaxDelay = _wordRecognitionWindowVM.MaxDelay,
					MinDuration = _wordRecognitionWindowVM.MinDuration,
					MaxDuration = _wordRecognitionWindowVM.MaxDuration,
					MinIntensity = _wordRecognitionWindowVM.MinIntensity,
					MaxIntensity = _wordRecognitionWindowVM.MaxIntensity,
				});
			}
			else
			{
				Words[SelectedWordIndex].Word = _wordRecognitionWindowVM.InputText;
				Words[SelectedWordIndex].Type = _wordRecognitionWindowVM.ShockType;
				Words[SelectedWordIndex].MinInitialDelay = _wordRecognitionWindowVM.MinInitialDelay;
				Words[SelectedWordIndex].MaxInitialDelay = _wordRecognitionWindowVM.MaxInitialDelay;
				Words[SelectedWordIndex].MinDelay = _wordRecognitionWindowVM.MinDelay;
				Words[SelectedWordIndex].MaxDelay = _wordRecognitionWindowVM.MaxDelay;
				Words[SelectedWordIndex].MinDuration = _wordRecognitionWindowVM.MinDuration;
				Words[SelectedWordIndex].MaxDuration = _wordRecognitionWindowVM.MaxDuration;
				Words[SelectedWordIndex].MinIntensity = _wordRecognitionWindowVM.MinIntensity;
				Words[SelectedWordIndex].MaxIntensity = _wordRecognitionWindowVM.MaxIntensity;
			}
		}

		
	}

	public int SelectedWordIndex
	{
		get => _selectedWordIndex;
		set
		{
			_selectedWordIndex = value;
			OnPropertyChanged(nameof(SelectedWordIndex));
			OnPropertyChanged(nameof(HasSelectedWord));
		}
	}

	public bool HasSelectedWord => SelectedWordIndex <= Words.Count && SelectedWordIndex > -1;
}
