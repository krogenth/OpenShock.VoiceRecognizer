using OpenShock.VoiceRecognizer.Common.Audio;
using OpenShock.VoiceRecognizer.Common.Enums;
using OpenShock.VoiceRecognizer.Configuration;
using OpenShock.VoiceRecognizer.UI.ViewModels.Enums;
using OpenShock.VoiceRecognizer.UI.ViewModels.Numbers;

namespace OpenShock.VoiceRecognizer.UI.ViewModels.Settings;

public class SettingsGeneralViewModel : BasedSettingsViewModel
{
	public RecognizerTypeSelectorViewModel RecognizerTypeSelectorVM { get; }
	public AudioDeviceSelectorViewModel InputDeviceSelectorVM { get; }
	public IntInputViewModel ListenPortSelectorVM { get; }
	public ShockCollarTypeSelectorViewModel ShockCollarTypeSelectorVM { get; }

	private RecognizerType _recognizerType;
	private string _inputDeviceID;
	private int _listenPort;
	private ShockCollarType _collarType;

	public SettingsGeneralViewModel()
	{
		_recognizerType = ConfigurationState.Instance!.General.Recognizer.Value;
		_inputDeviceID = ConfigurationState.Instance!.General.InputDeviceID.Value;
		_listenPort = ConfigurationState.Instance!.OSC.ListenPort.Value;
		_collarType = ConfigurationState.Instance!.General.CollarType.Value;

		RecognizerTypeSelectorVM = new(
			"Recognizer Type",
			_recognizerType
		);

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
		RecognizerTypeSelectorVM.EnumChanged += RecognizerChanged;
		InputDeviceSelectorVM.DeviceChanged += AudioDeviceChanged;
		ListenPortSelectorVM.ValueChanged += ListenPortChanged;
		ShockCollarTypeSelectorVM.EnumChanged += ShockCollarTypeChanged;
	}

	public override void SaveToConfigurationState()
	{
		ConfigurationState.Instance!.General.Recognizer.Value = _recognizerType;
		ConfigurationState.Instance!.General.InputDeviceID.Value = _inputDeviceID;
		ConfigurationState.Instance!.OSC.ListenPort.Value = _listenPort;
		ConfigurationState.Instance!.General.CollarType.Value = _collarType;
	}

	private void RecognizerChanged(object? sender, EnumChangedEventArgs<RecognizerType> e) =>
		_recognizerType = e.Value;

	private void AudioDeviceChanged(object? sender, AudioDeviceChangedEventArgs e)
	{
		switch (e.AudioDeviceType)
		{
			case AudioDeviceType.Input:
				_inputDeviceID = e.DeviceID;
				break;
		}
	}

	private void ListenPortChanged(object? sender, IntInputViewModel.NumberChangedEventArgs e) =>
		_listenPort = e.Value;

	private void ShockCollarTypeChanged(object? sender, EnumChangedEventArgs<ShockCollarType> e) =>
		_collarType = e.Value;
}
