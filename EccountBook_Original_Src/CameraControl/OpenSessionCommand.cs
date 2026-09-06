using System;
using EDSDKLib;

namespace CameraControl;

internal class OpenSessionCommand : Command
{
	public OpenSessionCommand(ref CameraModel model)
		: base(ref model)
	{
	}

	public override bool Execute()
	{
		uint err = 0u;
		err = EDSDK.EdsSetPropertyData(_model.Camera, 16777216u, 344198641, 4, 16778261u);
		err = EDSDK.EdsSetPropertyData(_model.Camera, 16777216u, 95646733, 4, 16778564u);
		err = EDSDK.EdsSetPropertyData(_model.Camera, 16777216u, 397354417, 4, 16778274u);
		err = EDSDK.EdsSetPropertyData(_model.Camera, 16777216u, 705434228, 4, 16778275u);
		err = EDSDK.EdsSetPropertyData(_model.Camera, 16777216u, 1068593547, 4, 16778289u);
		err = EDSDK.EdsSetPropertyData(_model.Camera, 16777216u, 1697663145, 4, 16778502u);
		err = EDSDK.EdsSetPropertyData(_model.Camera, 16777216u, 1294498291, 4, 16778566u);
		err = EDSDK.EdsSetPropertyData(_model.Camera, 16777216u, 551368201, 4, 16777732u);
		err = EDSDK.EdsSetPropertyData(_model.Camera, 16777216u, 1367279965, 4, 16778296u);
		err = EDSDK.EdsSetPropertyData(_model.Camera, 16777216u, 14759065, 4, 16778273u);
		err = EDSDK.EdsSetPropertyData(_model.Camera, 16777216u, 1373447830, 4, 16777238u);
		err = EDSDK.EdsSetPropertyData(_model.Camera, 16777216u, 16413175, 4, 16777239u);
		err = EDSDK.EdsSetPropertyData(_model.Camera, 16777216u, 158860912, 4, 16777240u);
		err = EDSDK.EdsSetPropertyData(_model.Camera, 16777216u, 472995419, 4, 16778334u);
		err = EDSDK.EdsSetPropertyData(_model.Camera, 16777216u, 517805750, 4, 16778352u);
		err = EDSDK.EdsSetPropertyData(_model.Camera, 16777216u, 1337216572, 4, 16778353u);
		err = EDSDK.EdsSetPropertyData(_model.Camera, 16777216u, 1550590130, 4, 16778354u);
		err = EDSDK.EdsSetPropertyData(_model.Camera, 16777216u, 329141533, 4, 16778355u);
		err = EDSDK.EdsSetPropertyData(_model.Camera, 16777216u, 13961478, 4, 16778356u);
		err = EDSDK.EdsSetPropertyData(_model.Camera, 16777216u, 1886745055, 4, 16778327u);
		err = EDSDK.EdsSetPropertyData(_model.Camera, 16777216u, 1144611223, 4, 16778333u);
		err = EDSDK.EdsOpenSession(_model.Camera);
		bool locked = false;
		err = EDSDK.EdsSetPropertyData(_model.Camera, 11u, 0, 4, 2u);
		if (err == 0)
		{
			err = EDSDK.EdsSendStatusCommand(_model.Camera, 0u, 0);
		}
		if (err == 0)
		{
			locked = true;
		}
		if (err == 0)
		{
			EDSDK.EdsCapacity Capacity = default(EDSDK.EdsCapacity);
			Capacity.NumberOfFreeClusters = int.MaxValue;
			Capacity.BytesPerSector = 4096;
			Capacity.Reset = 1;
			err = EDSDK.EdsSetCapacity(_model.Camera, Capacity);
		}
		if (locked)
		{
			err = EDSDK.EdsSendStatusCommand(_model.Camera, 1u, 0);
		}
		if (err != 0)
		{
			CameraEvent e = new CameraEvent(CameraEvent.Type.ERROR, (IntPtr)err);
			_model.NotifyObservers(e);
		}
		return true;
	}
}
