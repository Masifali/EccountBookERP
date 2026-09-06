using System.Windows.Forms;

namespace CameraControl;

public class InfoLabel : Label
{
	private delegate void _UpdateProperty(string infoText);

	protected void UpdateProperty(string infoText)
	{
		if (base.InvokeRequired)
		{
			Invoke(new _UpdateProperty(UpdateProperty), infoText);
		}
		else if (infoText != null)
		{
			Text = infoText;
		}
	}
}
