using System.Collections.Generic;
using System.Windows.Forms;
using EDSDKLib;

namespace CameraControl;

public class PropertyComboBox : ComboBox
{
	private delegate void _UpdateProperty(uint value);

	private delegate void _UpdatePropertyDesc(ref EDSDK.EdsPropertyDesc desc);

	protected Dictionary<uint, string> map = new Dictionary<uint, string>();

	protected void UpdateProperty(uint value)
	{
		string outString;
		if (base.InvokeRequired)
		{
			Invoke(new _UpdateProperty(UpdateProperty), value);
		}
		else if (map.TryGetValue(value, out outString) && !outString.Equals("unknown"))
		{
			if (base.Items.Count == 0)
			{
				base.Items.Add(outString);
			}
			base.SelectedItem = outString;
			if ((string)base.SelectedItem != outString && ToString().Contains("AeModeComboBox"))
			{
				base.Items.Clear();
				base.Items.Add(outString);
				base.SelectedItem = outString;
			}
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
		base.Items.Clear();
		for (int i = 0; i < desc.NumElements; i++)
		{
			if (map.TryGetValue((uint)desc.PropDesc[i], out var outString) && !outString.Equals("unknown"))
			{
				base.Items.Add(outString);
			}
		}
	}
}
