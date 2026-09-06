using System;
using EDSDKLib;

namespace CameraControl;

internal class TakePictureCommand : Command
{
	public TakePictureCommand(ref CameraModel model)
		: base(ref model)
	{
	}

	public override bool Execute()
	{
		uint err = EDSDK.EdsSendCommand(_model.Camera, 4u, 3);
		err = EDSDK.EdsSendCommand(_model.Camera, 4u, 0);
		switch (err)
		{
		case 129u:
		{
			CameraEvent e = new CameraEvent(CameraEvent.Type.DEVICE_BUSY, IntPtr.Zero);
			_model.NotifyObservers(e);
			return true;
		}
		default:
		{
			CameraEvent e2 = new CameraEvent(CameraEvent.Type.ERROR, (IntPtr)err);
			_model.NotifyObservers(e2);
			return true;
		}
		case 0u:
			_model.canDownloadImage = true;
			return true;
		}
	}
}
