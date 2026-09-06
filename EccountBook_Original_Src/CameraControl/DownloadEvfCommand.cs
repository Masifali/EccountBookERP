using System;
using System.Runtime.InteropServices;
using EDSDKLib;

namespace CameraControl;

internal class DownloadEvfCommand : Command
{
	public DownloadEvfCommand(ref CameraModel model)
		: base(ref model)
	{
	}

	public override bool Execute()
	{
		uint err = 0u;
		IntPtr evfImage = IntPtr.Zero;
		IntPtr stream = IntPtr.Zero;
		ulong bufferSize = 2097152uL;
		IntPtr evfDataSetPtr = IntPtr.Zero;
		IntPtr cameraPosPtr = IntPtr.Zero;
		if ((_model.EvfOutputDevice & 2) == 0)
		{
			return true;
		}
		err = EDSDK.EdsCreateMemoryStream(bufferSize, out stream);
		if (err == 0)
		{
			err = EDSDK.EdsCreateEvfImageRef(stream, out evfImage);
		}
		if (err == 0)
		{
			err = EDSDK.EdsDownloadEvfImage(_model.Camera, evfImage);
		}
		if (err == 0)
		{
			EVFDataSet dataset = default(EVFDataSet);
			dataset.stream = stream;
			EDSDK.EdsGetPropertyData(evfImage, 1287u, 0, out dataset.zoom);
			EDSDK.EdsGetPropertyData(evfImage, 1291u, 0, out dataset.imagePosition);
			EDSDK.EdsGetPropertyData(evfImage, 1345u, 0, out dataset.zoomRect);
			EDSDK.EdsGetPropertyData(evfImage, 1344u, 0, out dataset.sizeJpegLarge);
			EDSDK.EdsGetPropertyData(evfImage, 16778566u, 0, out dataset.visibleRect);
			_model.SizeJpegLarge = dataset.sizeJpegLarge;
			_model.SetPropertyRect(1345u, dataset.zoomRect);
			_model.SetPropertyRect(16778566u, dataset.visibleRect);
			evfDataSetPtr = Marshal.AllocHGlobal(Marshal.SizeOf(dataset));
			Marshal.StructureToPtr(dataset, evfDataSetPtr, fDeleteOld: false);
		}
		if (err == 0)
		{
			CameraEvent e4 = new CameraEvent(CameraEvent.Type.EVFDATA_CHANGED, evfDataSetPtr);
			_model.NotifyObservers(e4);
		}
		if (err == 0 && _model.RollPitch == 0)
		{
			EDSDK.EdsCameraPos cameraPos = default(EDSDK.EdsCameraPos);
			err = EDSDK.EdsGetPropertyData(evfImage, 16778564u, 0, out cameraPos);
			if (err == 0)
			{
				cameraPosPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cameraPos));
				Marshal.StructureToPtr(cameraPos, cameraPosPtr, fDeleteOld: false);
				CameraEvent e3 = new CameraEvent(CameraEvent.Type.ANGLEINFO, cameraPosPtr);
				_model.NotifyObservers(e3);
			}
		}
		if (stream != IntPtr.Zero)
		{
			EDSDK.EdsRelease(stream);
			stream = IntPtr.Zero;
		}
		if (evfImage != IntPtr.Zero)
		{
			EDSDK.EdsRelease(evfImage);
			evfImage = IntPtr.Zero;
		}
		if (evfDataSetPtr != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(evfDataSetPtr);
		}
		switch (err)
		{
		case 41218u:
			return false;
		case 129u:
		{
			CameraEvent e = new CameraEvent(CameraEvent.Type.DEVICE_BUSY, IntPtr.Zero);
			_model.NotifyObservers(e);
			return false;
		}
		default:
		{
			CameraEvent e2 = new CameraEvent(CameraEvent.Type.ERROR, (IntPtr)err);
			_model.NotifyObservers(e2);
			break;
		}
		case 0u:
			break;
		}
		return true;
	}
}
