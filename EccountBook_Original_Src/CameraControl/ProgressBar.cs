using System.Windows.Forms;

namespace CameraControl;

internal class ProgressBar : System.Windows.Forms.ProgressBar
{
	private delegate void _UpdateProperty(uint value);

	public ActionEvent.Command Command { get; set; }

	protected void UpdateProperty(uint value)
	{
		if (base.InvokeRequired)
		{
			Invoke(new _UpdateProperty(UpdateProperty), value);
		}
		else
		{
			base.Value = (int)value;
		}
	}
}
