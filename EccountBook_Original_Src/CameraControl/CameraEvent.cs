using System;

namespace CameraControl;

public class CameraEvent
{
	public enum Type
	{
		NONE,
		ERROR,
		DEVICE_BUSY,
		DOWNLOAD_START,
		DOWNLOAD_COMPLETE,
		EVFDATA_CHANGED,
		PROGRESS_REPORT,
		PROPERTY_CHANGED,
		PROPERTY_DESC_CHANGED,
		DELETE_START,
		DELETE_COMPLETE,
		PROGRESS,
		ANGLEINFO,
		MOUSE_CURSOR,
		SHUT_DOWN
	}

	private Type _type;

	private IntPtr _arg;

	public CameraEvent(Type type, IntPtr arg)
	{
		_type = type;
		_arg = arg;
	}

	public Type GetEventType()
	{
		return _type;
	}

	public IntPtr GetArg()
	{
		return _arg;
	}
}
