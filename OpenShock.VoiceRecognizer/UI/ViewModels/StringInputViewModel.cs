using System;

namespace OpenShock.VoiceRecognizer.UI.ViewModels;

public class StringInputViewModel(string title, string initialValue) : BaseViewModel
{
	private string? _text { get; set; } = initialValue;
	public string Title { get; private set; } = title;

	public void Reset() => Text = string.Empty;

	public string? Text
	{
		get => _text;
		set
		{
			_text = value;
			OnPropertyChanged(nameof(Text));
		}
	}
}
