using System.Threading;
using System.Windows.Forms;

namespace Architecture.WinApp;

public class WaitForm
{
	private frmWaitForm waitform;

	private Thread loadthread;

	public void Show()
	{
		loadthread = new Thread((ThreadStart)LoadingProcess);
		loadthread.IsBackground = true;
		loadthread.Start();
	}

	public void Show(Form parent)
	{
		loadthread = new Thread((ParameterizedThreadStart)LoadingProcess);
		loadthread.IsBackground = true;
		loadthread.Start(parent);
	}

	public void Close()
	{
		if (waitform != null && !waitform.IsDisposed)
		{
			try
			{
				waitform.Invoke(new ThreadStart(waitform.CloseWaitForm));
			}
			catch
			{
			}
			finally
			{
				waitform = null;
				loadthread = null;
			}
		}
		if (loadthread != null && loadthread.IsAlive)
		{
			loadthread.Join(500);
		}
		loadthread = null;
	}

	private void LoadingProcess()
	{
		waitform = new frmWaitForm();
		Application.Run(waitform);
	}

	private void LoadingProcess(object parent)
	{
		Form parent2 = parent as Form;
		waitform = new frmWaitForm(parent2);
		Application.Run(waitform);
	}
}
