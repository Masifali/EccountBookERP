using EDSDKLib;

namespace CameraControl;

internal class SwitchMirrorUpCommand : Command
{
	private uint _status;

	public SwitchMirrorUpCommand(ref CameraModel model, uint status)
		: base(ref model)
	{
		_status = status;
	}

	public override bool Execute()
	{
		if (_model.MirrorLockUpState == 2 || _model.isEvfEnable || _model.FixedMovie != 0)
		{
			return true;
		}
		if (_status == 1 && _model.MirrorLockUpState == 0)
		{
			uint mirrorUpSetting = 1u;
			EDSDK.EdsSetPropertyData(_model.Camera, 16778296u, 0, 4, mirrorUpSetting);
		}
		else if (_status == 0 && _model.MirrorLockUpState == 1)
		{
			uint mirrorUpSetting2 = 0u;
			EDSDK.EdsSetPropertyData(_model.Camera, 16778296u, 0, 4, mirrorUpSetting2);
		}
		return true;
	}
}
