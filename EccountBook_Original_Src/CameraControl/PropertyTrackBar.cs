using System.Windows.Forms;
using EDSDKLib;

namespace CameraControl;

public class PropertyTrackBar : TrackBar
{
	private delegate void _UpdateProperty(uint value);

	private delegate void _UpdatePropertyDesc(ref EDSDK.EdsPropertyDesc desc);

	protected void UpdateProperty(uint value)
	{
		if (base.InvokeRequired)
		{
			Invoke(new _UpdateProperty(UpdateProperty), value);
		}
		else if (base.CanSelect)
		{
			base.Value = (int)value;
		}
	}

	protected void UpdatePropertyDesc(ref EDSDK.EdsPropertyDesc desc)
	{
		if (base.InvokeRequired)
		{
			Invoke(new _UpdatePropertyDesc(UpdatePropertyDesc), desc);
			return;
		}
		base.Enabled = desc.NumElements != 0;
		if (desc.NumElements != 0)
		{
			base.Maximum = desc.PropDesc[0] - 1;
		}
	}
}
