namespace CameraControl;

internal class AfModeComboBox : PropertyComboBox, IObserver
{
	public AfModeComboBox()
	{
		map.Add(0u, "One-Shot AF");
		map.Add(1u, "AI Servo AF");
		map.Add(2u, "AI Focus AF");
		map.Add(3u, "Manual Focus");
		map.Add(uint.MaxValue, "unknown");
	}

	public void Update(Observable from, CameraEvent e)
	{
		CameraModel model = (CameraModel)from;
		CameraEvent.Type eventType = CameraEvent.Type.NONE;
		if (((eventType = e.GetEventType()) == CameraEvent.Type.PROPERTY_CHANGED || eventType == CameraEvent.Type.PROPERTY_DESC_CHANGED) && (int)e.GetArg() == 1028)
		{
			uint property = model.AFMode;
			if (eventType == CameraEvent.Type.PROPERTY_CHANGED)
			{
				UpdateProperty(property);
			}
		}
	}
}
