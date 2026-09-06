using System.Windows.Forms;

namespace Architecture.WinApp.Common;

public class clsExceptions
{
	private void ShowMessage(string message)
	{
		MessageBox.Show(message, "Ecountant", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
	}
}
