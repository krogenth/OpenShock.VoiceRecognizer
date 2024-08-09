using NAudio.CoreAudioApi;
using OpenShock.VoiceRecognizer.Common.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpenShock.VoiceRecognizer.UI.ViewModels;

public class AudioDeviceSelectorViewModel : BaseViewModel
{
	private int _selectedDeviceIndex;

	public event EventHandler<AudioDeviceChangedEventArgs>? DeviceChanged;

	public AudioDeviceType AudioDeviceType { get; private set; }
	public ObservableCollection<MMDevice> Devices { get; private set; }

	public AudioDeviceSelectorViewModel()
	{
		AudioDeviceType = AudioDeviceType.Input;
		Devices = new ObservableCollection<MMDevice>();
		SelectedDeviceIndex = 0;
	}

	public AudioDeviceSelectorViewModel(AudioDeviceType type, IEnumerable<MMDevice> devices, string defaultDeviceID)
	{
		AudioDeviceType = type;
		Devices = new ObservableCollection<MMDevice>(devices);

		SelectedDeviceIndex = defaultDeviceID != null ?
			Devices.IndexOf(Devices.FirstOrDefault(device => device.ID == defaultDeviceID)!) :
			0;
	}

	public int SelectedDeviceIndex
	{
		get => _selectedDeviceIndex;
		set
		{
			if (value < 0)
			{
				value = 0;
			}

			if (value >= Devices.Count)
			{
				value = Devices.Count - 1;
			}

			_selectedDeviceIndex = value;
			OnPropertyChanged(nameof(SelectedDeviceIndex));
			DeviceChanged?.Invoke(this, new AudioDeviceChangedEventArgs(AudioDeviceType, Devices.ElementAt(SelectedDeviceIndex).ID));
		}
	}
}

public class AudioDeviceChangedEventArgs(AudioDeviceType type, string id) : EventArgs
{
	public string DeviceID { get; set; } = id;
	public AudioDeviceType AudioDeviceType { get; set; } = type;
}
