using System;
using EDSDKLib;

namespace CameraControl;

internal class SetRemoteShootingCommand : Command
{
	private uint _parameter;

	public SetRemoteShootingCommand(ref CameraModel model, uint parameter)
		: base(ref model)
	{
		_parameter = parameter;
	}

	public override bool Execute()
	{
		if (EDSDK.EdsGetPropertyData(_model.Camera, 1541u, 0, out uint outPropertyData) != 0 || outPropertyData == 1)
		{
			return true;
		}
		uint err = EDSDK.EdsSendCommand(_model.Camera, 271u, (int)_parameter);
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
