using System;
using EDSDKLib;

namespace CameraControl;

internal class EvfAfModeComboBox : PropertyComboBox, IObserver
{
	private ActionSource _actionSource;

	private EDSDK.EdsPropertyDesc _desc;

	public void SetActionSource(ref ActionSource actionSource)
	{
		_actionSource = actionSource;
	}

	public EvfAfModeComboBox()
	{
		map.Add(0u, "Quick mode");
		map.Add(1u, "1-point AF");
		map.Add(2u, "Face+Tracking");
		map.Add(3u, "FlexiZone - Multi");
		map.Add(4u, "Zone AF");
		map.Add(5u, "Expand AF area");
		map.Add(6u, "Expand AF area: Around");
		map.Add(7u, "Large Zone AF: Horizontal");
		map.Add(8u, "Large Zone AF: Vertical");
		map.Add(9u, "Catch AF");
		map.Add(10u, "Spot AF");
		map.Add(11u, "Flexible Zone AF 1");
		map.Add(12u, "Flexible Zone AF 2");
		map.Add(13u, "Flexible Zone AF 3");
		map.Add(14u, "Whole area AF");
		map.Add(uint.MaxValue, "unknown");
	}

	protected override void OnSelectionChangeCommitted(EventArgs e)
	{
		if (base.SelectedItem != null)
		{
			uint key = (uint)_desc.PropDesc[SelectedIndex];
			_actionSource.FireEvent(ActionEvent.Command.SET_EVF_AFMODE, (IntPtr)key);
		}
	}

	public void Update(Observable from, CameraEvent e)
	{
		CameraModel model = (CameraModel)from;
		CameraEvent.Type eventType = CameraEvent.Type.NONE;
		if (((eventType = e.GetEventType()) != CameraEvent.Type.PROPERTY_CHANGED && eventType != CameraEvent.Type.PROPERTY_DESC_CHANGED) || (int)e.GetArg() != 1294)
		{
			return;
		}
		uint property = model.EvfAFMode;
		switch (eventType)
		{
		case CameraEvent.Type.PROPERTY_CHANGED:
			if (9 == property)
			{
				EDSDK.EdsSetPropertyData(model.Camera, 1294u, 0, 4, 2);
			}
			else
			{
				UpdateProperty(property);
			}
			break;
		case CameraEvent.Type.PROPERTY_DESC_CHANGED:
			_desc = model.EvfAFModeDesc;
			UpdatePropertyDesc(ref _desc);
			if (9 != property)
			{
				UpdateProperty(property);
			}
			break;
		}
	}
}
