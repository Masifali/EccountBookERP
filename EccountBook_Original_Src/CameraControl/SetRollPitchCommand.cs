using System;
using EDSDKLib;

namespace CameraControl;

internal class SetRollPitchCommand : Command
{
	private uint _status;

	public SetRollPitchCommand(ref CameraModel model, uint status)
		: base(ref model)
	{
		_status = status;
	}

	public override bool Execute()
	{
		uint rollPitch = _model.RollPitch;
		if (!_model.isEvfEnable)
		{
			return true;
		}
		uint err = EDSDK.EdsSendCommand(_model.Camera, 265u, (int)rollPitch);
		if (rollPitch == 1)
		{
			_model.NotifyObservers(new CameraEvent(CameraEvent.Type.ANGLEINFO, IntPtr.Zero));
		}
		switch (err)
		{
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
