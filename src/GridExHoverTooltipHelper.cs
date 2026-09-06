using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using Janus.Windows.GridEX;

public class GridExHoverTooltipHelper : IDisposable
{
	private readonly GridEX _grid;

	private GridEXRow _previousRow;

	public GridExHoverTooltipHelper(GridEX grid)
	{
		_grid = grid ?? throw new ArgumentNullException("grid");
		_grid.CellToolTip = (CellToolTip)2;
		_grid.SelectionChanged += Grid_SelectionChanged;
	}

	private void Grid_SelectionChanged(object sender, EventArgs e)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		ClearRowTooltips(_previousRow);
		GridEXRow currentRow = _grid.CurrentRow;
		if (currentRow == null)
		{
			return;
		}
		using (Graphics g = ((Control)(object)_grid).CreateGraphics())
		{
			foreach (GridEXCell item in (IEnumerable)currentRow.Cells)
			{
				GridEXCell cell = item;
				if (string.IsNullOrEmpty(cell.Text))
				{
					cell.ToolTipText = null;
					continue;
				}
				bool isTruncated = TextRenderer.MeasureText(g, cell.Text, ((Control)(object)_grid).Font, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.SingleLine).Width > cell.Column.Width;
				cell.ToolTipText = (isTruncated ? cell.Text : null);
			}
		}
		_previousRow = currentRow;
	}

	private void ClearRowTooltips(GridEXRow row)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		if (row == null || (int)row.RowType != 0 || row.Cells == null || row.Cells.Count == 0)
		{
			return;
		}
		for (int i = 0; i < row.Cells.Count; i++)
		{
			GridEXCell cell = row.Cells[i];
			if (cell != null)
			{
				cell.ToolTipText = null;
			}
		}
	}

	public void Dispose()
	{
		_grid.SelectionChanged -= Grid_SelectionChanged;
	}
}
