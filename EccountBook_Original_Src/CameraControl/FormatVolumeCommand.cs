using System;
using System.Windows.Forms;
using EDSDKLib;

namespace CameraControl;

internal class FormatVolumeCommand : Command
{
	private IntPtr _volume;

	public FormatVolumeCommand(ref CameraModel model, ref IntPtr volume)
		: base(ref model)
	{
		_volume = volume;
	}

	public override bool Execute()
	{
		_ = _model.Camera;
		uint err = 0u;
		if (err == 0)
		{
			if (MessageBox.Show("Format the memory card?", "CameraControl", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
			{
				return true;
			}
			if (err == 0)
			{
				err = EDSDK.EdsFormatVolume(_volume);
			}
		}
		return true;
	}
}
