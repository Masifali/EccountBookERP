using System;
using System.Windows.Forms;

namespace CameraControl;

internal class ActionRadioButton : RadioButton
{
	private ActionSource _actionSource;

	public ActionEvent.Command Command { get; set; }

	public void SetActionSource(ref ActionSource actionSource)
	{
		_actionSource = actionSource;
	}

	protected override void OnClick(EventArgs e)
	{
		_actionSource.FireEvent(Command, IntPtr.Zero);
		base.OnCheckedChanged(e);
	}
}
