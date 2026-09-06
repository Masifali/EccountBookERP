using System;
using System.Windows.Forms;
using EDSDKLib;

namespace CameraControl;

internal class ZoomTrackBar : PropertyTrackBar, IObserver
{
	private ActionSource _actionSource;

	private EDSDK.EdsPropertyDesc _desc;

	public void SetActionSource(ref ActionSource actionSource)
	{
		_actionSource = actionSource;
	}

	public ZoomTrackBar()
	{
		base.MouseUp += _MouseUp;
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		((HandledMouseEventArgs)e).Handled = true;
	}

	private void _MouseUp(object sender, MouseEventArgs e)
	{
		_actionSource.FireEvent(ActionEvent.Command.SET_ZOOM, (IntPtr)base.Value);
	}

	public void Update(Observable from, CameraEvent e)
	{
		CameraModel model = (CameraModel)from;
		CameraEvent.Type eventType = CameraEvent.Type.NONE;
		if (((eventType = e.GetEventType()) == CameraEvent.Type.PROPERTY_CHANGED || eventType == CameraEvent.Type.PROPERTY_DESC_CHANGED) && (int)e.GetArg() == 1536 && !model.isTypeDS)
		{
			uint property = model.Zoom;
			switch (eventType)
			{
			case CameraEvent.Type.PROPERTY_CHANGED:
				UpdateProperty(property);
				break;
			case CameraEvent.Type.PROPERTY_DESC_CHANGED:
				_desc = model.ZoomDesc;
				UpdatePropertyDesc(ref _desc);
				UpdateProperty(property);
				break;
			}
		}
	}
}
