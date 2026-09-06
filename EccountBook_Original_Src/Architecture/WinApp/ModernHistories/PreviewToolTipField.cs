using System.Windows.Forms;

namespace Architecture.WinApp.ModernHistories;

public class PreviewToolTipField
{
	public string Caption { get; set; }

	public Control ValueControl { get; set; }

	public int CaptionWidth { get; set; } = 160;


	public int ValueWidth { get; set; } = 230;


	public int RowHeight { get; set; } = 28;


	public bool IsHighlight { get; set; }

	public Padding CaptionPadding { get; set; } = new Padding(1);


	public Padding ValuePadding { get; set; } = new Padding(1);

}
