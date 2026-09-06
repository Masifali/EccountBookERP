using System;
using EDSDKLib;

namespace CameraControl;

internal class AspectComboBox : PropertyComboBox, IObserver
{
	private ActionSource _actionSource;

	private EDSDK.EdsPropertyDesc _desc;

	public void SetActionSource(ref ActionSource actionSource)
	{
		_actionSource = actionSource;
	}

	public AspectComboBox()
	{
		map.Add(0u, "Full-frame");
		map.Add(1u, "1:1(aspect ratio)");
		map.Add(2u, "4:3(aspect ratio)");
		map.Add(7u, "16:9(aspect ratio)");
		map.Add(13u, "1.6x(crop)");
	}

	protected override void OnSelectionChangeCommitted(EventArgs e)
	{
		if (base.SelectedItem != null)
		{
			uint key = (uint)_desc.PropDesc[SelectedIndex];
			_actionSource.FireEvent(ActionEvent.Command.SET_ASPECT, (IntPtr)key);
		}
	}

	public void Update(Observable from, CameraEvent e)
	{
		CameraModel model = (CameraModel)from;
		CameraEvent.Type eventType = CameraEvent.Type.NONE;
		if (((eventType = e.GetEventType()) == CameraEvent.Type.PROPERTY_CHANGED || eventType == CameraEvent.Type.PROPERTY_DESC_CHANGED) && (int)e.GetArg() == 16778289)
		{
			uint property = model.Aspect;
			switch (eventType)
			{
			case CameraEvent.Type.PROPERTY_CHANGED:
				UpdateProperty(property);
				break;
			case CameraEvent.Type.PROPERTY_DESC_CHANGED:
				_desc = model.AspectDesc;
				UpdatePropertyDesc(ref _desc);
				UpdateProperty(property);
				break;
			}
		}
	}
}
