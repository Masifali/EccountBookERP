using System;
using EDSDKLib;

namespace CameraControl;

internal class SwitchStillMovieCommand : Command
{
	private uint _status;

	public SwitchStillMovieCommand(ref CameraModel model, uint status)
		: base(ref model)
	{
		_status = status;
	}

	public override bool Execute()
	{
		uint err = 0u;
		if (_model.FixedMovie == 0 && _status == 264)
		{
			return true;
		}
		if (_status == 263)
		{
			err = EDSDK.EdsSetPropertyData(_model.Camera, 11u, 0, 4, 1u);
		}
		else
		{
			err = EDSDK.EdsSetPropertyData(_model.Camera, 11u, 0, 4, 2u);
			EDSDK.EdsCapacity Capacity = default(EDSDK.EdsCapacity);
			Capacity.NumberOfFreeClusters = int.MaxValue;
			Capacity.BytesPerSector = 4096;
			Capacity.Reset = 1;
			err = EDSDK.EdsSetCapacity(_model.Camera, Capacity);
		}
		err = EDSDK.EdsSendCommand(_model.Camera, _status, 0);
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
			return true;
		}
	}
}
