using System;
using Architecture.WinApp.Lab;
using EDSDKLib;

namespace CameraControl;

internal class DownloadCommand : Command
{
	private IntPtr _directoryItem = IntPtr.Zero;

	private CameraEvent _event;

	public DownloadCommand(ref CameraModel model, ref IntPtr inRef)
		: base(ref model)
	{
		_directoryItem = inRef;
	}

	~DownloadCommand()
	{
		if (_directoryItem != IntPtr.Zero)
		{
			EDSDK.EdsRelease(_directoryItem);
			_directoryItem = IntPtr.Zero;
		}
	}

	public override bool Execute()
	{
		if (!_model.canDownloadImage)
		{
			return true;
		}
		uint err = 0u;
		IntPtr stream = IntPtr.Zero;
		err = EDSDK.EdsGetDirectoryItemInfo(_directoryItem, out var dirItemInfo);
		if (err == 0)
		{
			CameraEvent e3 = new CameraEvent(CameraEvent.Type.DOWNLOAD_START, stream);
			_model.NotifyObservers(e3);
		}
		if (err == 0)
		{
			string FullPath = InvLabPurchaseAnalysis.saveDirectory + "\\" + (InvLabPurchaseAnalysis.UniqPathAnalysis = $"{Guid.NewGuid()}.jpg");
			dirItemInfo.szFileName = FullPath;
			err = EDSDK.EdsCreateFileStream(dirItemInfo.szFileName, EDSDK.EdsFileCreateDisposition.CreateAlways, EDSDK.EdsAccess.ReadWrite, out stream);
		}
		if (err == 0)
		{
			err = EDSDK.EdsSetProgressCallback(stream, ProgressFunc, EDSDK.EdsProgressOption.Periodically, stream);
		}
		if (err == 0)
		{
			err = EDSDK.EdsDownload(_directoryItem, dirItemInfo.Size, stream);
		}
		if (err == 0)
		{
			err = EDSDK.EdsDownloadComplete(_directoryItem);
		}
		if (_directoryItem != IntPtr.Zero)
		{
			err = EDSDK.EdsRelease(_directoryItem);
			_directoryItem = IntPtr.Zero;
		}
		if (stream != IntPtr.Zero)
		{
			err = EDSDK.EdsRelease(stream);
			stream = IntPtr.Zero;
		}
		if (err == 0)
		{
			CameraEvent e2 = new CameraEvent(CameraEvent.Type.DOWNLOAD_COMPLETE, stream);
			_model.NotifyObservers(e2);
		}
		if (err != 0)
		{
			CameraEvent e = new CameraEvent(CameraEvent.Type.ERROR, (IntPtr)err);
			_model.NotifyObservers(e);
		}
		return true;
	}

	private uint ProgressFunc(uint inPercent, IntPtr inContext, ref bool outCancel)
	{
		_event = new CameraEvent(CameraEvent.Type.PROGRESS, (IntPtr)inPercent);
		_model.NotifyObservers(_event);
		return 0u;
	}
}
