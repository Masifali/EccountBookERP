using EDSDKLib;

namespace CameraControl;

internal class SetRecCommand : Command
{
	private uint _status;

	public SetRecCommand(ref CameraModel model, uint status)
		: base(ref model)
	{
		_status = status;
	}

	public override bool Execute()
	{
		if (_model.FixedMovie == 1)
		{
			EDSDK.EdsSetPropertyData(_model.Camera, 1296u, 0, 4, _status);
		}
		return true;
	}
}
