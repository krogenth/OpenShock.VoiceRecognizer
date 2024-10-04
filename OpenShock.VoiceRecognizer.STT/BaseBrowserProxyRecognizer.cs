using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using Microsoft.Extensions.Logging;
using OpenShock.VoiceRecognizer.Configuration;
using OpenShock.VoiceRecognizer.IO.Logger;
using OpenShock.VoiceRecognizer.IO.Resources;
using System.Linq;

namespace OpenShock.VoiceRecognizer.STT;

public abstract class BaseBrowserProxyRecognizer : BaseRecognizer
{
	protected HttpListener? _listener = null;
	protected Thread? _thread = null;
	protected bool _shouldExit = false;
	protected int _port = 0;
	protected BrowserSpeech? _lastDetectedSpeech = null;
	protected List<string> _speechDetectionOnEnd = [];
	protected List<string> _pendingJavascript = [];
	protected List<string> _languageChunks = [];
	protected List<int> _speechDetectionUtterances = [];
	protected List<string> _speechDetectionVoices = [];
	protected CancellationTokenSource? _workerThreadTokenSource;

	public void StartProxy(int port)
	{
		_port = port;
		_listener = new();
		_listener.Prefixes.Add(string.Format("http://127.0.0.1:{0}/", port));
		_listener.Start();

		_workerThreadTokenSource = new();
		_thread = new(() => WorkerThread(_workerThreadTokenSource.Token))
		{
			IsBackground = true,
		};
		_thread.Start();
	}

	public void StopProxy()
	{
		try
		{
			_workerThreadTokenSource!.Cancel();
			_thread?.Join();
			_listener?.Stop();
		}
		catch (Exception) { }
		finally
		{
			_workerThreadTokenSource?.Dispose();
			_workerThreadTokenSource = null;
			_listener = null;
			_thread = null;
		}
	}

	protected abstract void OpenBrowser(int port);

	protected abstract void DetectBrowser();

	protected abstract void CloseBrowser();

	protected void AddJavascriptCommand(string command) => _pendingJavascript.Add(command);

	protected string GetPendingJavasacriptCommands()
	{
		StringBuilder sb = new();
		foreach (string command in _pendingJavascript)
		{
			sb.AppendLine(command);
		}

		_pendingJavascript.Clear();
		return sb.ToString();
	}

	protected void HandleProxyData(string request)
	{
		if (string.IsNullOrEmpty(request))
		{
			return;
		}

		if (request.StartsWith("SpeechSynthesisIdle:"))
		{
			return;
		}

		if (request.StartsWith("SpeechDetectionGetLanguagesChunkFirst:"))
		{
			// more language chunk stuff, only appends...
		}
		else if (request.StartsWith("SpeechDetectionGetLanguagesChunkLast:"))
		{
			// more language chunk stuff, only appends...
		}
		else if (request.StartsWith("SpeechDetectionGetLanguagesChunk:"))
		{

		}
		else if (request.StartsWith("SpeechDetectionGetLanguages:"))
		{

		}
		else
		{
			if (request.StartsWith("SpeechDetectionGetResult:"))
			{
				string item = HttpUtility.UrlDecode(request["SpeechDetectionGetResult:".Length..]);
				_lastDetectedSpeech = JsonSerializer.Deserialize<BrowserSpeech>(item);

				// unfortuantely data might be broken up, so we need to put it together
				string text = string.Empty;
				foreach (var alt in
					from result in _lastDetectedSpeech?.Results
					from alt in result.Alternatives
					select alt
				)
				{
					text += alt.Transcript;
				}

				OnRecognizedSpeech(text);
				return;
			}

			if (request.StartsWith("SpeechDetectionInit:"))
			{
				_speechDetectionOnEnd.Clear();
				AddJavascriptCommand("console.log(\"Browser Proxy Initialized\")");
				return;
			}

			if (!request.StartsWith("SpeechSynthesisCancel:"))
			{
				if (request.StartsWith("SpeechSynthesisOnEnd:"))
				{
					string item = HttpUtility.UrlDecode(request["SpeechSynthesisOnEnd:".Length..]);
					//_speechDetectionOnEnd.Add(item);
					return;
				}

				if (request.StartsWith("SpeechSynthesisCreateSpeechSynthesisUtterance:"))
				{
					if (int.TryParse(request["SpeechSynthesisCreateSpeechSynthesisUtterance:".Length..], out int item))
					{
						_speechDetectionUtterances.Add(item);
						return;
					}
				}
				else if (request.StartsWith("SpeechSynthesisGetVoices:"))
				{
					string text = request["SpeechSynthesisGetVoices:".Length..];
					if (!string.IsNullOrEmpty(text))
					{
						string item = HttpUtility.UrlDecode(text);
						_speechDetectionVoices.Add(item);
						return;
					}
				}
				else if (!request.StartsWith("SpeechSynthesisSetPitch:") && !request.StartsWith("SpeechSynthesisSetRate:") && !request.StartsWith("SpeechSynthesisSetText:"))
				{
					request.StartsWith("SpeechSynthesisSpeak:");
				}
			}
		}
	}

	protected void WorkerThread(CancellationToken token)
	{
		while (!_shouldExit)
		{
			if (token.IsCancellationRequested)
			{
				return;
			}

			if (_listener is null)
			{
				Thread.Sleep(0);
				continue;
			}

			HttpListenerContext? context = null;
			try
			{
				var log = Logger.GetLogger<BaseBrowserProxyRecognizer>();
				context = _listener?.GetContext();

				if (context is null)
				{
					continue;
				}

				string text = string.Empty;
				string? contextUrlLocalPath = context!.Request.Url?.LocalPath;
				if (!string.IsNullOrEmpty(contextUrlLocalPath))
				{
					log?.LogInformation("got context path: {contextUrlLocalPath}", contextUrlLocalPath);
					if (contextUrlLocalPath.EndsWith('/'))
					{
						_lastDetectedSpeech = null;
						_speechDetectionOnEnd.Clear();
						DetectBrowser();
						byte[] bytes = [];
						try
						{
							text = EmbeddedResources.ReadAllText("OpenShock.VoiceRecognizer/Assets/Proxies/browserproxy.html");
							bytes = Encoding.UTF8.GetBytes(text);
							text = Encoding.UTF8.GetString(bytes).Replace("__PROXY_PORT__", _port.ToString());
						}
						catch (Exception) { }
					}
					else if (contextUrlLocalPath.EndsWith("/crossdomain.xml"))
					{
						text = $"<cross-domain-policy>{Environment.NewLine}";
						text += $"<allow-access-from-domain=\"*\"/>{Environment.NewLine}";
						text += $"</cross-domain-policy>{Environment.NewLine}";
					}
					else if (contextUrlLocalPath.EndsWith("/ProxyData"))
					{
						DetectBrowser();
						var message = HttpUtility.ParseQueryString(context!.Request.Url?.Query ?? string.Empty)["message"];
						if (!string.IsNullOrEmpty(message))
						{
							byte[] messageBytes = Convert.FromBase64String(message);
							string @string = Encoding.UTF8.GetString(messageBytes);
							HandleProxyData(@string);
						}

						text = GetPendingJavasacriptCommands();
					}
					else if (contextUrlLocalPath.EndsWith("/CloseBrowserTab"))
					{
						CloseBrowser();
					}
					else if (contextUrlLocalPath.EndsWith("/CloseProxy"))
					{
						_shouldExit = true;
					}
					else if (contextUrlLocalPath.EndsWith("/OpenBrowserTab"))
					{
						_pendingJavascript.Clear();
						OpenBrowser(ConfigurationState.Instance!.BrowserProxy.ProxyPort.Value);
					}
					else if (contextUrlLocalPath.EndsWith("/SetProxyPort"))
					{
						// do stuff
					}
					else if (contextUrlLocalPath.EndsWith("/SpeechDetectionGetResult"))
					{
						if (_lastDetectedSpeech is not null)
						{
							text = JsonSerializer.Serialize(_lastDetectedSpeech);
							_lastDetectedSpeech = null;
						}
					}
					else if (contextUrlLocalPath.EndsWith("/SpeechDetectionInit"))
					{
						_lastDetectedSpeech = null;
					}
					else if (contextUrlLocalPath.EndsWith("/SpeechDetectionStartRecognition"))
					{
						AddJavascriptCommand("WebGLSpeechDetectionPlugin.Start()");
					}
					else if (contextUrlLocalPath.EndsWith("/SpeechDetectionStopRecognition"))
					{
						AddJavascriptCommand("WebGLSpeechDetectionPlugin.Stop()");
					}
					else if (contextUrlLocalPath.EndsWith("/SpeechDetectionAbort"))
					{
						AddJavascriptCommand("WebGLSpeechDetectionPlugin.Abort()");
					}
					else if (contextUrlLocalPath.EndsWith("/SpeechDetectionGetLanguages"))
					{
						AddJavascriptCommand("WebGLSpeechDetectionPlugin.GetLanguages()");
					}
					else if (contextUrlLocalPath.EndsWith("/SpeechDetectionSetLanguage"))
					{
						_lastDetectedSpeech = null;
						string languageCode = HttpUtility.ParseQueryString(context!.Request.Url?.Query ?? string.Empty)["lang"]!;
						if (languageCode is not null)
						{
							AddJavascriptCommand($"WebGLSpeechDetectionPlugin.SetLanguage(\"{languageCode}\")");
						}
					}
				}

				byte[] textBytes = Encoding.UTF8.GetBytes(text);
				context!.Response.ContentEncoding = Encoding.UTF8;
				context!.Response.AddHeader("ContentType", "utf8");
				context!.Response.OutputStream.Write(textBytes, 0, textBytes.Length);
				context!.Response.OutputStream.Flush();
			}
			catch (Exception ex)
			{
				var log = Logger.GetLogger<BaseBrowserProxyRecognizer>();
				log?.LogError("Got doinked: {Message}", ex.Message);
			}
			finally
			{
				try
				{
					context?.Response.Close();
				}
				catch (Exception) { }
			}
		}
	}
}

public class BrowserSpeechResultAlternative
{
	[JsonPropertyName("confidence")]
	public float Confidence { get; set; }
	[JsonPropertyName("transcript")]
	public string Transcript { get; set; } = string.Empty;
}

public class BrowserSpeechResult
{
	[JsonPropertyName("isFinal")]
	public bool IsFinal { get; set; }
	[JsonPropertyName("alternatives")]
	public List<BrowserSpeechResultAlternative> Alternatives { get; set; } = [];
	[JsonPropertyName("length")]
	public int Length { get; set; }
}

public class BrowserSpeech
{
	[JsonPropertyName("results")]
	public List<BrowserSpeechResult> Results { get; set; } = [];
}
