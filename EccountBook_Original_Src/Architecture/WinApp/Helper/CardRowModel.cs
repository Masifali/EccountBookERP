using System.Collections.Generic;

namespace Architecture.WinApp.Helper;

public class CardRowModel
{
	public List<CardCellModel> ColList { get; set; }

	public CardCellModel Col1 { get; set; }

	public CardCellModel Col2 { get; set; }

	public CardCellModel Col3 { get; set; }

	public CardCellModel Col4 { get; set; }

	public int RowHeight { get; set; } = 22;

}
