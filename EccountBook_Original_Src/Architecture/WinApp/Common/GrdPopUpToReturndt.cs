using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Janus.Data;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.Common;

public class GrdPopUpToReturndt : Form
{
	public DataTable dtReturn = new DataTable();

	private readonly bool returnFullDt;

	private IContainer components;

	internal GridEX grd;

	public GrdPopUpToReturndt(DataTable dt, bool selection = false, bool returnFullGrdDt = false, List<string> hiddenCols = null, Dictionary<string, EditType> editCols = null)
	{
		InitializeComponent();
		try
		{
			returnFullDt = returnFullGrdDt;
			dtReturn = dt.Clone();
			grd.DataSource = dt;
			grd.RetrieveStructure();
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 3, 2, hiddenCols, editCols);
			if (selection)
			{
				GridEX_Helper.AddSelectButton(grd, "Select", 30, 0);
			}
			grd.ColumnAutoResize = false;
			((Control)(object)grd).Focus();
			SendKeys.Send("{Up}");
			SendKeys.Send("{Right}");
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error: " + ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (returnFullDt)
			{
				AddRowsToReturn(grd.GetDataRows());
				Hide();
				return;
			}
			GridEXRow[] checkedRows = grd.GetCheckedRows();
			if (checkedRows.Any())
			{
				AddRowsToReturn(checkedRows);
				Hide();
				return;
			}
			GridEXRow row = grd.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				AddRowToReturn(row);
				Close();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void AddRowsToReturn(IEnumerable<GridEXRow> rows)
	{
		foreach (GridEXRow row in rows)
		{
			AddRowToReturn(row);
		}
	}

	private void AddRowToReturn(GridEXRow row)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		if (row == null)
		{
			return;
		}
		DataRow newRow = dtReturn.NewRow();
		foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
		{
			string colName = item.Key;
			if (dtReturn.Columns.Contains(colName))
			{
				newRow[colName] = row.Cells[colName].Value;
			}
		}
		dtReturn.Rows.Add(newRow);
	}

	private void GrdPopUp_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			else if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyCode == Keys.Return)
			{
				GridEXRow row = grd.CurrentRow;
				if (row != null && (int)row.RowType == 0 && !returnFullDt)
				{
					grd_DoubleClick(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		this.grd = new GridEX();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		base.SuspendLayout();
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		this.grd.ColumnAutoResize = true;
		this.grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(511, 415);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(511, 415);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		base.KeyPreview = true;
		base.Name = "GrdPopUp";
		this.Text = "GrdPopUp";
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(GrdPopUp_KeyDown);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		base.ResumeLayout(false);
	}
}
