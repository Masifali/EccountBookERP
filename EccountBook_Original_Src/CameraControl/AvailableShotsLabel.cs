namespace CameraControl;

internal class AvailableShotsLabel : InfoLabel, IObserver
{
	public void Update(Observable from, CameraEvent e)
	{
		CameraEvent.Type eventType = CameraEvent.Type.NONE;
		if ((eventType = e.GetEventType()) == CameraEvent.Type.PROPERTY_CHANGED && (int)e.GetArg() == 1034 && eventType == CameraEvent.Type.PROPERTY_CHANGED)
		{
			CameraModel obj = (CameraModel)from;
			string infoText = "";
			infoText = obj.AvailableShot.ToString();
			UpdateProperty(infoText);
		}
	}
}
