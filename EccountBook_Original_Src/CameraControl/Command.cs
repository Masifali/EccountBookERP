namespace CameraControl;

internal abstract class Command
{
	protected CameraModel _model;

	public Command(ref CameraModel model)
	{
		_model = model;
	}

	public CameraModel GetCameraModel()
	{
		return _model;
	}

	public virtual bool Execute()
	{
		return true;
	}
}
