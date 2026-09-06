using System;
using System.Runtime.InteropServices;
using EDSDKLib;

namespace CameraControl;

internal class CameraEventListener
{
	public static uint HandleObjectEvent(uint inEvent, IntPtr inRef, IntPtr inContext)
	{
		CameraController controller = (CameraController)GCHandle.FromIntPtr(inContext).Target;
		if (inEvent == 520)
		{
			FireEvent(ref controller, ActionEvent.Command.DOWNLOAD, inRef);
		}
		else if (inRef != IntPtr.Zero)
		{
			EDSDK.EdsRelease(inRef);
		}
		return 0u;
	}

	public static uint HandlePropertyEvent(uint inEvent, uint inPropertyID, uint inParam, IntPtr inContext)
	{
		CameraController controller = (CameraController)GCHandle.FromIntPtr(inContext).Target;
		switch (inEvent)
		{
		case 257u:
			FireEvent(ref controller, ActionEvent.Command.GET_PROPERTY, (IntPtr)inPropertyID);
			break;
		case 258u:
			FireEvent(ref controller, ActionEvent.Command.GET_PROPERTYDESC, (IntPtr)inPropertyID);
			break;
		}
		return 0u;
	}

	public static uint HandleStateEvent(uint inEvent, uint inParameter, IntPtr inContext)
	{
		CameraController controller = (CameraController)GCHandle.FromIntPtr(inContext).Target;
		if (inEvent == 769)
		{
			FireEvent(ref controller, ActionEvent.Command.SHUT_DOWN, IntPtr.Zero);
		}
		return 0u;
	}

	private static void FireEvent(ref CameraController controller, ActionEvent.Command command, IntPtr arg)
	{
		ActionEvent e = new ActionEvent(command, arg);
		controller.ActionPerformed(e);
	}
}
