using System;
using EDSDKLib;

namespace CameraControl;

internal class CloseSessionCommand : Command
{
	public CloseSessionCommand(ref CameraModel model)
		: base(ref model)
	{
	}

	public override bool Execute()
	{
		uint err = EDSDK.EdsCloseSession(_model.Camera);
		if (err != 0)
		{
			CameraEvent e = new CameraEvent(CameraEvent.Type.ERROR, (IntPtr)err);
			_model.NotifyObservers(e);
		}
		return true;
	}
}
