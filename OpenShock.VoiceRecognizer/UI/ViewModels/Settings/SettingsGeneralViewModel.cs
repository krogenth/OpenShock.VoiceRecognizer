using OpenShock.VoiceRecognizer.Common.Audio;
using OpenShock.VoiceRecognizer.Common.Enums;
using OpenShock.VoiceRecognizer.Configuration;
using OpenShock.VoiceRecognizer.UI.ViewModels.Enums;

namespace OpenShock.VoiceRecognizer.UI.ViewModels.Settings;

public class SettingsGeneralViewModel : BasedSettingsViewModel
{
	public AudioDeviceSelectorViewModel InputDeviceSelectorVM { get; }
	public NumberInputViewModel ListenPortSelectorVM { get; }
	public ShockCollarTypeSelectorViewModel ShockCollarTypeSelectorVM { get; }

	private string _inputDeviceID;
	private int _listenPort;
	private ShockCollarType _collarType;

	public SettingsGeneralViewModel()
	{
		_inputDeviceID = ConfigurationState.Instance!.Audio.InputDeviceID.Value;
		_listenPort = ConfigurationState.Instance!.OSC.ListenPort.Value;
		_collarType = ConfigurationState.Instance!.Shock.CollarType.Value;

		InputDeviceSelectorVM = new(
			AudioDeviceType.Input,
			AudioDevices.GetInputAudioDevices(),
			_inputDeviceID
		);

		ListenPortSelectorVM = new(
			"OSC Listen Port",
			_listenPort
		);

		ShockCollarTypeSelectorVM = new(
			"Shock Collar Type",
			_collarType
		);

		AttachEventHandlers();
	}

	private void AttachEventHandlers()
	{
		InputDeviceSelectorVM.DeviceChanged += AudioDeviceChanged;
		ListenPortSelectorVM.NumberValueChanged += ListenPortChanged;
		ShockCollarTypeSelectorVM.EnumChanged += ShockCollarTypeChanged;
	}

	public override void SaveToConfigurationState()
	{
		ConfigurationState.Instance!.Audio.InputDeviceID.Value = _inputDeviceID;
		ConfigurationState.Instance!.OSC.ListenPort.Value = _listenPort;
		ConfigurationState.Instance!.Shock.CollarType.Value = _collarType;
	}

	private void AudioDeviceChanged(object? sender, AudioDeviceChangedEventArgs e)
	{
		switch (e.AudioDeviceType)
		{
			case AudioDeviceType.Input:
				_inputDeviceID = e.DeviceID;
				break;
		}
	}

	private void ListenPortChanged(object? sender, NumberValueChangedEventArgs e) =>
		_listenPort = e.Value;

	private void ShockCollarTypeChanged(object? sender, EnumChangedEventArgs<ShockCollarType> e) =>
		_collarType = e.Value;
}
