using System.ComponentModel;
using OpenShock.VoiceRecognizer.Configuration;

namespace OpenShock.VoiceRecognizer.UI.ViewModels.Settings;

public class SettingsOpenShockViewModel : BaseViewModel
{
	public StringInputViewModel SendHostSelectorVM { get; set; }
	public NumberInputViewModel SendPortSelectorVM { get; set; }
	public StringInputViewModel ListenIntensityEndpointSelectorVM { get; set; }
	public StringInputViewModel SendShockEndpointSelectorVM { get; set; }
	public StringInputViewModel SendVibrateEndpointSelectorVM { get; set; }

	public SettingsOpenShockViewModel()
	{
		SendHostSelectorVM = new(
			"Host",
			ConfigurationState.Instance!.OpenShock.SendHost.Value
		);
		SendPortSelectorVM = new(
			"Port",
			ConfigurationState.Instance!.OpenShock.SendPort.Value
		);
		ListenIntensityEndpointSelectorVM = new(
			"Intensity Listen Endpoint",
			ConfigurationState.Instance!.OpenShock.ExternalListenSetIntensityEndpoint.Value
		);
		SendShockEndpointSelectorVM = new(
			"Send Shock Endpoint",
			ConfigurationState.Instance!.OpenShock.ExternalSendStartShockEndpoint.Value
		);
		SendVibrateEndpointSelectorVM = new(
			"Send Vibrate Endpoint",
			ConfigurationState.Instance!.OpenShock.ExternalSendStartVibrationEndpoint.Value
		);

		AttachEventHandlers();
	}

	private void AttachEventHandlers()
	{
		SendHostSelectorVM.PropertyChanged += OnSendHostChanged;
		SendPortSelectorVM.PropertyChanged += OnSendPortChanged;
		ListenIntensityEndpointSelectorVM.PropertyChanged += OnListenIntensityEndpointChanged;
		SendShockEndpointSelectorVM.PropertyChanged += OnSendShockEndpointChanged;
		SendVibrateEndpointSelectorVM.PropertyChanged += OnSendVibrateEndpointChanged;
	}

	private void OnSendHostChanged(object? sender, PropertyChangedEventArgs e)
		=> ConfigurationState.Instance!.OpenShock.SendHost.Value = SendHostSelectorVM.Text!;

	private void OnSendPortChanged(object? sender, PropertyChangedEventArgs e)
		=> ConfigurationState.Instance!.OpenShock.SendPort.Value = SendPortSelectorVM.Value!;

	private void OnListenIntensityEndpointChanged(object? sender, PropertyChangedEventArgs e)
		=> ConfigurationState.Instance!.OpenShock.ExternalListenSetIntensityEndpoint.Value = ListenIntensityEndpointSelectorVM.Text!;

	private void OnSendShockEndpointChanged(object? sender, PropertyChangedEventArgs e)
		=> ConfigurationState.Instance!.OpenShock.ExternalSendStartShockEndpoint.Value = SendShockEndpointSelectorVM.Text!;

	private void OnSendVibrateEndpointChanged(object? sender, PropertyChangedEventArgs e)
		=> ConfigurationState.Instance!.OpenShock.ExternalSendStartVibrationEndpoint.Value = SendVibrateEndpointSelectorVM.Text!;
}
