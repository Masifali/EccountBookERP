using System;
using EDSDKLib;

namespace CameraControl;

internal class MeteringModeComboBox : PropertyComboBox, IObserver
{
	private ActionSource _actionSource;

	private EDSDK.EdsPropertyDesc _desc;

	public void SetActionSource(ref ActionSource actionSource)
	{
		_actionSource = actionSource;
	}

	public MeteringModeComboBox()
	{
		map.Add(1u, "Spot metering");
		map.Add(3u, "Evaluative metering");
		map.Add(4u, "Partial metering");
		map.Add(5u, "Center-weighted average");
		map.Add(uint.MaxValue, "unknown");
	}

	protected override void OnSelectionChangeCommitted(EventArgs e)
	{
		if (base.SelectedItem != null)
		{
			uint key = (uint)_desc.PropDesc[SelectedIndex];
			_actionSource.FireEvent(ActionEvent.Command.SET_METERING_MODE, (IntPtr)key);
		}
	}

	public void Update(Observable from, CameraEvent e)
	{
		CameraModel model = (CameraModel)from;
		CameraEvent.Type eventType = CameraEvent.Type.NONE;
		if (((eventType = e.GetEventType()) == CameraEvent.Type.PROPERTY_CHANGED || eventType == CameraEvent.Type.PROPERTY_DESC_CHANGED) && (int)e.GetArg() == 1027)
		{
			uint property = model.MeteringMode;
			switch (eventType)
			{
			case CameraEvent.Type.PROPERTY_CHANGED:
				UpdateProperty(property);
				break;
			case CameraEvent.Type.PROPERTY_DESC_CHANGED:
				_desc = model.MeteringModeDesc;
				UpdatePropertyDesc(ref _desc);
				UpdateProperty(property);
				break;
			}
		}
	}
}
