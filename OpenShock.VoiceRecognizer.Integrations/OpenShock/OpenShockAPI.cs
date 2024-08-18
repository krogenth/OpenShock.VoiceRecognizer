using OpenShock.SDK.CSharp;
using OpenShock.SDK.CSharp.Models;
using OpenShock.VoiceRecognizer.Common;
using OpenShock.VoiceRecognizer.Configuration;

namespace OpenShock.VoiceRecognizer.Integrations.OpenShock;

public class OpenShockAPI
{
	public OpenShockApiClient? Client { get; private set; } = null;

	public OpenShockAPI()
	{
		GenerateNewClient();
		AttachHandlers();
	}

	public async Task<IEnumerable<ShockerResponse>> GetShockers()
	{
		var shockerList = new List<ShockerResponse>();
		var shockers = await Client.GetOwnShockers();
		if (shockers.IsT0)
		{
			foreach (var shockerCollection in shockers.AsT0.Value)
			{
				shockerList.AddRange(shockerCollection.Shockers);
			}
		}

		return shockerList;
	}

	public async Task GetDeviceGateway()
	{

	}

	private void AttachHandlers() { }
		//ConfigurationState.Instance!.OpenShock.APIKey.ValueChanged += OnAPIKeyChanged;

	//private void OnAPIKeyChanged(object? sender, ValueChangedEventArgs<string> e) =>
	//	GenerateNewClient();

	private void GenerateNewClient()
	{
		/*var apiKey = ConfigurationState.Instance!.OpenShock.APIKey.Value;
		if (!string.IsNullOrEmpty(apiKey))
		{
			Client = new(new ApiClientOptions { Token = apiKey });
		}*/
	}
}
