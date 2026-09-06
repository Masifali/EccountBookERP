namespace CameraControl;

internal class BatteryLebelLabel : InfoLabel, IObserver
{
	public void Update(Observable from, CameraEvent e)
	{
		CameraEvent.Type eventType = CameraEvent.Type.NONE;
		if ((eventType = e.GetEventType()) == CameraEvent.Type.PROPERTY_CHANGED && (int)e.GetArg() == 8 && eventType == CameraEvent.Type.PROPERTY_CHANGED)
		{
			CameraModel model = (CameraModel)from;
			string infoText = "AC power";
			if (-1 != (int)model.BatteryLebel)
			{
				infoText = model.BatteryLebel + "%";
			}
			UpdateProperty(infoText);
		}
	}
}
