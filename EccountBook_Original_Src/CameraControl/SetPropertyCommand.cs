using System;
using System.Runtime.InteropServices;
using EDSDKLib;

namespace CameraControl;

internal class SetPropertyCommand : Command
{
	private uint _propertyID;

	private object _data;

	public SetPropertyCommand(ref CameraModel model, uint propertyID, object data)
		: base(ref model)
	{
		_propertyID = propertyID;
		_data = data;
	}

	public override bool Execute()
	{
		uint err = 0u;
		err = EDSDK.EdsSetPropertyData(_model.Camera, _propertyID, 0, Marshal.SizeOf(_data), _data);
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
			break;
		}
		case 0u:
			break;
		}
		return true;
	}
}
