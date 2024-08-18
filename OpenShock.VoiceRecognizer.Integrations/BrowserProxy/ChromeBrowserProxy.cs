using System.Net;
using System.Text;
using System.Web;
using NAudio.CoreAudioApi;

namespace OpenShock.VoiceRecognizer.Integrations.BrowserProxy;

public class ChromeBrowserProxy : BaseBrowserProxy
{
	public override void StartProxy(int port)
	{
		_port = port;
		_listener = new();
		_listener.Prefixes.Add($"http://*:{port}/");
		_listener.Start();
		_thread = new(new ThreadStart(WorkerThread));
		_thread.Start();
	}

	public override void StopProxy()
	{
		try
		{
			_listener!.Abort();
		}
		catch (Exception) { }

		try
		{
			_listener!.Stop();
		}
		catch(Exception) { }
	}

	public override void CloseBrowser()
	{

	}

	public override void DetectBrowser()
	{

	}

	protected override void WorkerThread()
	{
		while (!_shouldExit)
		{
			if (_listener is null)
			{
				Thread.Sleep(1000);
				continue;
			}

			HttpListenerContext? context = null;
			try
			{
				context = _listener!.GetContext();
				string text = string.Empty;
				string? contextUrlLocalPath = context!.Request?.Url?.LocalPath;
				if (!string.IsNullOrEmpty(contextUrlLocalPath))
				{
					if (contextUrlLocalPath.EndsWith('/'))
					{
						_speechDetectionResults.Clear();
						_speechDetectionOnEnd.Clear();
						DetectBrowser();
						byte[] bytes = [];
						try
						{
							using FileStream stream = File.OpenRead("proxy.html");
							using StreamReader reader = new(stream, Encoding.UTF8);
							text = reader.ReadToEnd();
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
						var message = HttpUtility.ParseQueryString(context!.Request?.Url?.Query ?? string.Empty)["message"];
						if (!string.IsNullOrEmpty(message))
						{
							byte[] messageBytes = Convert.FromBase64String(message);
							string @string = Encoding.UTF8.GetString(messageBytes);
							//HandleProxyData(@string);
						}
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
						// pending javascript
						CloseBrowser();
					}
					else if (contextUrlLocalPath.EndsWith("/SetProxyPort"))
					{
						// do stuff
					}
					else if (contextUrlLocalPath.EndsWith("/SpeechDetectionGetResult"))
					{
						if (_speechDetectionResults.Count > 0)
						{
							text = _speechDetectionResults.First();
							_speechDetectionResults.RemoveAt(0);
						}
					}
					else if (contextUrlLocalPath.EndsWith("/SpeechDetectionInit"))
					{
						_speechDetectionResults.Clear();
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
						_speechDetectionResults.Clear();
						string languageCode = HttpUtility.ParseQueryString(context.Request!.Url!.Query)["lang"]!;
						if (languageCode is not null)
						{
							AddJavascriptCommand($"WebGLSpeechDetectionPlugin.SetLanguage(\"{languageCode}\")");
						}
					}
				}

				byte[] textBytes = Encoding.UTF8.GetBytes(text);
				context.Response.ContentEncoding = Encoding.UTF8;
				context.Response.AddHeader("ContentType", "utf8");
				context.Response.OutputStream.Write(textBytes, 0, textBytes.Length);
				context.Response.OutputStream.Flush();
			}
			catch (Exception) { }
		}
	}
}
