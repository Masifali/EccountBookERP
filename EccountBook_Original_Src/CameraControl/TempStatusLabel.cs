namespace CameraControl;

internal class TempStatusLabel : InfoLabel, IObserver
{
	public void Update(Observable from, CameraEvent e)
	{
		CameraEvent.Type eventType = CameraEvent.Type.NONE;
		if ((eventType = e.GetEventType()) == CameraEvent.Type.PROPERTY_CHANGED && (int)e.GetArg() == 16778261 && eventType == CameraEvent.Type.PROPERTY_CHANGED)
		{
			CameraModel model = (CameraModel)from;
			string[] infoText = new string[8] { "Normal", "Warning", "FramerateDown", "DisableLiveview", "DisableRelease", "StillQualityWarning", "RestrictionMovieRecording", "unknown" };
			if ((model.TempStatus & 0xFFFF0000u) == 131072)
			{
				UpdateProperty(infoText[6]);
			}
			else if (infoText.Length > model.TempStatus)
			{
				UpdateProperty(infoText[model.TempStatus]);
			}
			else
			{
				UpdateProperty(infoText[infoText.Length - 1]);
			}
		}
	}
}
