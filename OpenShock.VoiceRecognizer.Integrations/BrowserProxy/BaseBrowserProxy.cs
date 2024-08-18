using System.Net;

namespace OpenShock.VoiceRecognizer.Integrations.BrowserProxy;

public abstract class BaseBrowserProxy
{
	protected HttpListener? _listener = null;
	protected Thread? _thread = null;
	protected bool _shouldExit = false;
	protected int _port = 0;
	protected List<string> _speechDetectionResults = [];
	protected List<string> _speechDetectionOnEnd = [];
	protected List<string> _pendingJavascript = [];

	public BaseBrowserProxy() { }

	public abstract void StartProxy(int port);

	public abstract void StopProxy();

	public abstract void DetectBrowser();

	public abstract void CloseBrowser();

	public void AddJavascriptCommand(string command) => _pendingJavascript.Add(command);

	protected abstract void WorkerThread();
}
