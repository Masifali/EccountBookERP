using System.Threading;

namespace CameraControl;

internal class DownloadProgressBar : ProgressBar, IObserver
{
	public void Update(Observable from, CameraEvent e)
	{
		_ = (CameraModel)from;
		if (e.GetEventType() == CameraEvent.Type.PROGRESS)
		{
			uint percentage2 = (uint)(int)e.GetArg();
			UpdateProperty(percentage2);
		}
		if (e.GetEventType() == CameraEvent.Type.DOWNLOAD_COMPLETE)
		{
			Thread.Sleep(1000);
			uint percentage = 0u;
			UpdateProperty(percentage);
		}
	}
}
