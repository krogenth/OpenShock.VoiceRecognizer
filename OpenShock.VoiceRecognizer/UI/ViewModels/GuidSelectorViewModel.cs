using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpenShock.VoiceRecognizer.UI.ViewModels;

public class GuidSelectorViewModel : BaseViewModel
{
	private int _selectedGuidIndex;

	public event EventHandler<GuidChangedEventArgs>? GuidChanged;

	public string Title { get; private set; }
	public ObservableCollection<Guid> Guids { get; private set; }


	public GuidSelectorViewModel(string title, IEnumerable<Guid> guids, Guid initialGuid)
	{
		Title = title;
		Guids = new ObservableCollection<Guid>(guids);
		SelectedGuidIndex = Guids.IndexOf(Guids.FirstOrDefault(guid => guid == initialGuid));
	}

	public void UpdateGuids(IEnumerable<Guid> guids)
	{
		Guids.Clear();
		foreach (var guid in guids)
		{
			Guids.Add(guid);
		}

		SelectedGuidIndex = SelectedGuidIndex;
	}

	public int SelectedGuidIndex
	{
		get => _selectedGuidIndex;
		set
		{
			if (Guids.Count == 0)
			{
				return;
			}

			if (value >= Guids.Count)
			{
				value = Guids.Count - 1;
			}

			if (value < 0)
			{
				value = 0;
			}

			_selectedGuidIndex = value;
			OnPropertyChanged(nameof(SelectedGuidIndex));
			GuidChanged?.Invoke(this, new GuidChangedEventArgs(Guids.ElementAt(value)));
		}
	}

}
public class GuidChangedEventArgs(Guid value) : EventArgs
{
	public Guid Value { get; private set; } = value;
}
