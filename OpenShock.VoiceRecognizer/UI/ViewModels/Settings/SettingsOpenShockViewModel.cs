using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OpenShock.VoiceRecognizer.Configuration;
using OpenShock.VoiceRecognizer.Integrations.OpenShock;
using static OpenShock.VoiceRecognizer.Integrations.OpenShock.DevicesUpdatedEventArgs;

namespace OpenShock.VoiceRecognizer.UI.ViewModels.Settings;

public class SettingsOpenShockViewModel : BasedSettingsViewModel
{
	public StringInputViewModel APIKeySelectorVM { get; set; }
	public GuidSelectorViewModel DeviceIDSelectorVM { get; set; }
	public GuidSelectorViewModel ShockerIDSelectorVM { get; set; }

	private string _apiKey;
	private Guid _deviceID;
	private Guid _shockerID;

	public SettingsOpenShockViewModel()
	{
		OpenShockAPI.Instance.DevicesUpdated += OnDevicesChanged;
		OpenShockAPI.Instance.ShockersUpdated += OnShockersUpdated;
		OpenShockAPI.Instance.RefreshShockers();

		_apiKey = ConfigurationState.Instance!.OpenShock.APIKey.Value;
		_deviceID = ConfigurationState.Instance!.OpenShock.DeviceID.Value;
		_shockerID = ConfigurationState.Instance!.OpenShock.ShockerID.Value;

		APIKeySelectorVM = new(
			"API Key",
			_apiKey
		);
		DeviceIDSelectorVM = new(
			"Device",
			[],
			_deviceID
		);
		ShockerIDSelectorVM = new(
			"Shocker",
			[],
			_shockerID
		);

		AttachEventHandlers();
	}

	private void AttachEventHandlers()
	{
		APIKeySelectorVM.PropertyChanged += OnAPIKeyChanged;
		DeviceIDSelectorVM.GuidChanged += OnDeviceIDChanged;
		ShockerIDSelectorVM.GuidChanged += OnShockerIDChanged;
	}

	private void OnDevicesChanged(object? sender, DevicesUpdatedEventArgs e)
	{
		IEnumerable<Guid> devices = [];
		foreach ( var device in e.Devices)
		{
			devices = devices.Append(device.Id);
		}

		DeviceIDSelectorVM.UpdateGuids(devices);
	}

	private void OnShockersUpdated(object? sender, ShockersUpdatedEventArgs e)
	{
		IEnumerable<Guid> shockers = [];
		foreach (var shocker in e.Shockers)
		{
			shockers = shockers.Append(shocker.Id);
		}

		ShockerIDSelectorVM.UpdateGuids(shockers);
	}

	public override void SaveToConfigurationState()
	{
		ConfigurationState.Instance!.OpenShock.APIKey.Value = _apiKey;
		ConfigurationState.Instance!.OpenShock.DeviceID.Value = _deviceID;
		ConfigurationState.Instance!.OpenShock.ShockerID.Value = _shockerID;
	}

	private void OnAPIKeyChanged(object? sender, PropertyChangedEventArgs e) =>
		_apiKey = APIKeySelectorVM.Text!;

	private void OnDeviceIDChanged(object? sender, GuidChangedEventArgs e) =>
		_deviceID = e.Value;

	private void OnShockerIDChanged(object? sender, GuidChangedEventArgs e) =>
		_shockerID = e.Value;
}
