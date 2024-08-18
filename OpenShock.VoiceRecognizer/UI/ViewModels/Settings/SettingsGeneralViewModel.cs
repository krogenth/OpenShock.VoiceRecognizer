using OpenShock.VoiceRecognizer.Common.Audio;
using OpenShock.VoiceRecognizer.Common.Enums;
using OpenShock.VoiceRecognizer.Configuration;
using OpenShock.VoiceRecognizer.UI.ViewModels.Enums;

namespace OpenShock.VoiceRecognizer.UI.ViewModels.Settings;

public class SettingsGeneralViewModel : BaseViewModel
{
	public AudioDeviceSelectorViewModel InputDeviceSelectorVM { get; }
	public NumberInputViewModel ListenPortSelectorVM { get; }
	public ShockCollarTypeSelectorViewModel ShockCollarTypeSelectorVM { get; }

	public SettingsGeneralViewModel()
	{
		InputDeviceSelectorVM = new(
			AudioDeviceType.Input,
			AudioDevices.GetInputAudioDevices(),
			ConfigurationState.Instance!.Audio.InputDeviceID.Value
		);

		ListenPortSelectorVM = new(
			"OSC Listen Port",
			ConfigurationState.Instance!.OSC.ListenPort.Value
		);

		ShockCollarTypeSelectorVM = new(
			"Shock Collar Type",
			ConfigurationState.Instance!.Shock.CollarType.Value
		);

		AttachEventHandlers();
	}

	private void AttachEventHandlers()
	{
		InputDeviceSelectorVM.DeviceChanged += AudioDeviceChanged;
		ListenPortSelectorVM.NumberValueChanged += ListenPortChanged;
		ShockCollarTypeSelectorVM.EnumChanged += ShockCollarTypeChanged;
	}

	private void AudioDeviceChanged(object? sender, AudioDeviceChangedEventArgs e)
	{
		switch (e.AudioDeviceType)
		{
			case AudioDeviceType.Input:
				ConfigurationState.Instance!.Audio.InputDeviceID.Value = e.DeviceID;
				break;
		}
	}

	private void ListenPortChanged(object? sender, NumberValueChangedEventArgs e) =>
		ConfigurationState.Instance!.OSC.ListenPort.Value = e.Value;

	private void ShockCollarTypeChanged(object? sender, EnumChangedEventArgs<ShockCollarType> e) =>
		ConfigurationState.Instance!.Shock.CollarType.Value = e.Value;
}
