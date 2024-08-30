using Serilog;
using OpenShock.SDK.CSharp;
using OpenShock.SDK.CSharp.Live;
using OpenShock.SDK.CSharp.Models;
using OpenShock.VoiceRecognizer.Common;
using OpenShock.VoiceRecognizer.Configuration;
using OpenShock.VoiceRecognizer.IO.Logger;
using OpenShock.SDK.CSharp.Hub;

namespace OpenShock.VoiceRecognizer.Integrations.OpenShock;

public class OpenShockAPI : IDisposable
{
	public event EventHandler<DevicesUpdatedEventArgs>? DevicesUpdated;
	public event EventHandler<ShockersUpdatedEventArgs>? ShockersUpdated;
	public OpenShockApiClient? Client { get; private set; } = null;
	public OpenShockHubClient? HubClient { get; private set; } = null;
	public LcgResponse? Gateway { get; private set; } = null;
	public OpenShockLiveControlClient? ControlClient { get; private set; } = null;

	public IReadOnlyCollection<ResponseDeviceWithShockers> Devices = [];
	public IReadOnlyCollection<ShockerResponse> Shockers = [];

	public static OpenShockAPI Instance { get; private set; } = new();

	public OpenShockAPI()
	{
		GenerateNewClients();

		if (ConfigurationState.Instance!.OpenShock.DeviceID.Value != Guid.Empty)
		{
			GenerateNewGatewayAndControlClient(ConfigurationState.Instance!.OpenShock.DeviceID.Value);
		}
		AttachHandlers();
	}

	public async Task RefreshShockers()
	{
		if (Client is null)
		{
			return;
		}

		var response = await Client!.GetOwnShockers();

		response.Switch(success =>
		{
			Devices = success.Value;
			Shockers = success.Value.SelectMany(x => x.Shockers).ToArray();
			DevicesUpdated?.Invoke(this, new DevicesUpdatedEventArgs(Devices));
			ShockersUpdated?.Invoke(this, new ShockersUpdatedEventArgs(Shockers));
		},
		error => Log.Logger.Error("Could not authenticate with OpenShock API"));
	}

	public async Task SendVibrate(Guid deviceId, ushort duration, byte intensity)
	{
		var controls = new List<SDK.CSharp.Hub.Models.Control>
		{
			new()
			{
				Id = deviceId,
				Duration = duration,
				Intensity = intensity,
				Type = ControlType.Vibrate,
			}
		};

		await HubClient?.Control(controls);
	}
		

	public async Task SendShock(Guid deviceId, ushort duration, byte intensity)
	{
		var controls = new List<SDK.CSharp.Hub.Models.Control>
		{
			new()
			{
				Id = deviceId,
				Duration = duration,
				Intensity = intensity,
				Type = ControlType.Shock,
			}
		};

		await HubClient?.Control(controls);
	}

	public async Task SendStop(Guid deviceId, ushort duration, byte intensity)
	{
		var controls = new List<SDK.CSharp.Hub.Models.Control>
		{
			new()
			{
				Id = deviceId,
				Duration = duration,
				Intensity = intensity,
				Type = ControlType.Stop,
			}
		};

		await HubClient?.Control(controls);
	}

	private void AttachHandlers()
	{
		ConfigurationState.Instance!.OpenShock.APIKey.ValueChanged += OnAPIKeyChanged;
		ConfigurationState.Instance!.OpenShock.DeviceID.ValueChanged += OnDeviceIDChanged;
	}

	private void OnAPIKeyChanged(object? _, ValueChangedEventArgs<string> e)
	{
		GenerateNewClients();
		_ = GenerateNewGatewayAndControlClient(ConfigurationState.Instance!.OpenShock.DeviceID.Value);
	}

	private void OnDeviceIDChanged(object? _, ValueChangedEventArgs<Guid> e)
	{
		_ = GenerateNewGatewayAndControlClient(ConfigurationState.Instance!.OpenShock.DeviceID.Value);
	}

	private void GenerateNewClients()
	{
		GenerateNewClient();
		GenerateNewHubClient();
		RefreshShockers();
	}

	private async Task GenerateNewGatewayAndControlClient(Guid deviceId)
	{
		if (Client is not null)
		{
			await GenerateNewGateway(deviceId).ConfigureAwait(false);
			await GenerateNewControlClient(deviceId).ConfigureAwait(false);
		}
	}

	private void GenerateNewClient()
	{
		var apiKey = ConfigurationState.Instance!.OpenShock.APIKey.Value;
		if (!string.IsNullOrEmpty(apiKey))
		{
			Client = new OpenShockApiClient(new ApiClientOptions { Token = apiKey });
		}
	}

	private void GenerateNewHubClient()
	{
		var apiKey = ConfigurationState.Instance!.OpenShock.APIKey.Value;
		if (!string.IsNullOrEmpty(apiKey))
		{
			HubClient = new OpenShockHubClient(new HubClientOptions { Token = apiKey });
			HubClient.StartAsync();
		}
	}

	private async Task GenerateNewGateway(Guid deviceId)
	{
		if (Client is null)
		{
			return;
		}

		var gateway = await Client!.GetDeviceGateway(deviceId).ConfigureAwait(false);
		if (gateway.IsT1)
		{
			Log.Logger.Error("Failed to get gateway, make sure you use a valid device id");
		}

		if (gateway.IsT2)
		{
			Log.Logger.Error("Device is offline");
		}

		if (gateway.IsT3)
		{
			Log.Logger.Error("Device not connected to gateway");
		}

		Gateway = gateway.AsT0.Value;
	}

	public async Task GenerateNewControlClient(Guid deviceId)
	{
		if (Gateway is null)
		{
			await GenerateNewGateway(deviceId).ConfigureAwait(false);
		}

		ControlClient = new OpenShockLiveControlClient(
			Gateway!.Gateway,
			deviceId,
			ConfigurationState.Instance!.OpenShock.APIKey.Value,
			Logger.GetLogger<OpenShockLiveControlClient>()
		);
		await ControlClient.InitializeAsync();
	}

	public void Dispose()
	{
		ControlClient?.DisposeAsync().ConfigureAwait(false);
	}
}

public class DevicesUpdatedEventArgs(IEnumerable<ResponseDeviceWithShockers> devices) : EventArgs
{
	public IEnumerable<ResponseDeviceWithShockers> Devices { get; private set; } = devices;
}

public class ShockersUpdatedEventArgs(IEnumerable<ShockerResponse> shockers) : EventArgs
{
	public IEnumerable<ShockerResponse> Shockers { get; private set; } = shockers;
}
