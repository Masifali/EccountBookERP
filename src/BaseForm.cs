using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.WinApp.Helper;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Common
{

public class BaseForm : Form
{
	private ToolStrip btnTS = new ToolStrip();

	private ToolStripButton btnNew = new ToolStripButton();

	private ToolStripButton btnOpen = new ToolStripButton();

	private ToolStripButton btnSave = new ToolStripButton();

	private ToolStripButton btnPreview = new ToolStripButton();

	private ToolStripButton btnDelete = new ToolStripButton();

	private ToolStripButton btnReset = new ToolStripButton();

	private ToolStripButton btnClose = new ToolStripButton();

	protected ActionMode CurrentActivity = (ActionMode)6;

	protected Control focusControl = new Control();

	protected List<Grid> lstGrid = new List<Grid>();

	public void InitializeForm()
	{
		try
		{
			InitializeControlEvents();
			InitializeButtonEvents();
			InitializeGrid();
			ResetControl();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void InitializeControlEvents()
	{
		try
		{
			foreach (Control item in base.Controls)
			{
				if (item.GetType() == typeof(TextBox))
				{
					item.Enter += txt_Enter;
					item.PreviewKeyDown += txt_PreviewKeyDown;
				}
				else if (item.GetType() == typeof(MaskedTextBox))
				{
					item.Enter += mtb_Enter;
					item.Leave += mtb_Leave;
					item.PreviewKeyDown += mtb_PreviewKeyDown;
				}
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	public void InitializeButtonEvents()
	{
		try
		{
			btnTS = base.Controls.Find("tsbControlBar", searchAllChildren: false).FirstOrDefault() as ToolStrip;
			if (btnTS != null)
			{
				btnNew = btnTS.Items["btnNew"] as ToolStripButton;
				if (btnNew != null)
				{
					btnNew.Click += btnNew_Click;
				}
				btnOpen = btnTS.Items["btnOpen"] as ToolStripButton;
				if (btnOpen != null)
				{
					btnOpen.Click += btnOpen_Click;
				}
				btnSave = btnTS.Items["btnSave"] as ToolStripButton;
				if (btnSave != null)
				{
					btnSave.Click += btnSave_Click;
				}
				btnPreview = btnTS.Items["btnPreview"] as ToolStripButton;
				if (btnPreview != null)
				{
					btnPreview.Click += btnPreview_Click;
				}
				btnDelete = btnTS.Items["btnDelete"] as ToolStripButton;
				if (btnDelete != null)
				{
					btnDelete.Click += btnDelete_Click;
				}
				btnReset = btnTS.Items["btnReset"] as ToolStripButton;
				if (btnReset != null)
				{
					btnReset.Click += btnReset_Click;
				}
				btnClose = btnTS.Items["btnClose"] as ToolStripButton;
				if (btnClose != null)
				{
					btnClose.Click += btnClose_Click;
				}
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	public virtual void PreviewClick()
	{
	}

	public void btnPreview_Click(object sender, EventArgs e)
	{
		try
		{
			PreviewClick();
		}
		catch (Exception)
		{
		}
	}

	public void ResetGrid()
	{
		foreach (Control item in base.Controls)
		{
			if (!(item.GetType() == typeof(DataGridView)))
			{
				continue;
			}
			DataGridView dg = (DataGridView)item;
			if (dg != null)
			{
				int GetGridId = Convert.ToInt32(dg.Tag);
				Grid obj = lstGrid.Find((Grid x) => x.GridId == GetGridId);
				if (obj != null)
				{
					GridHelper.Grid_Reset(dg, obj.InitializeRows);
				}
			}
		}
	}

	public void InitializeGrid()
	{
		lstGrid = new List<Grid>();
		foreach (Control item in base.Controls)
		{
			if (item.GetType() == typeof(DataGridView))
			{
				GridLayout((DataGridView)item);
			}
		}
		foreach (Control item2 in base.Controls)
		{
			if (!(item2.GetType() == typeof(TabControl)))
			{
				continue;
			}
			foreach (TabPage tabPage in (item2 as TabControl).TabPages)
			{
				foreach (Control ctl in tabPage.Controls)
				{
					if (ctl.GetType() == typeof(DataGridView))
					{
						GridLayout((DataGridView)ctl);
					}
				}
			}
		}
	}

	private void GridLayout(DataGridView dg)
	{
		if (dg != null)
		{
			int GetGridId = Convert.ToInt32(dg.Tag);
			if (GetGridId == 0)
			{
				throw new Exception("Grid Id Not Define in Tag Property");
			}
			Grid obj = new GenericRepository<Grid>().GetById((long)GetGridId);
			List<SqlParameter> lstPrm = new List<SqlParameter>();
			lstPrm.Add(new SqlParameter("@GridId", GetGridId));
			obj.GridDetails = new GenericRepository<GridDetail>().GetBySearch(lstPrm);
			lstGrid.Add(obj);
			GridHelper.Grid_Structure(dg, obj);
			if (obj.IsReadOnly)
			{
				dg.CellClick += Grid_List_CellClick;
				return;
			}
			dg.CellContentClick += Grid_CellContentClick;
			dg.CellEndEdit += Grid_CellEndEdit;
			dg.CellDoubleClick += Grid_CellDoubleClick;
			dg.DataError += Grid_ControlDataError;
			dg.KeyDown += Grid_ControlKeyDown;
			dg.UserDeletingRow += Grid_ControlUserDeletingRow;
			dg.UserDeletedRow += Grid_ControlUserDeletedRow;
			dg.EditingControlShowing += Grid_EditingControlShowing;
		}
	}

	private void Grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
	{
		if (e.Control is ComboBox cmb)
		{
			cmb.SelectedIndex = -1;
			cmb.DropDownStyle = ComboBoxStyle.DropDownList;
			cmb.Font = new Font("Tahoma", 8.5f);
			cmb.SelectionChangeCommitted -= cmb_SelectedIndexChanged;
			cmb.SelectionChangeCommitted += cmb_SelectedIndexChanged;
			cmb.PreviewKeyDown -= cmb_PreviewKeyDown;
			cmb.PreviewKeyDown += cmb_PreviewKeyDown;
		}
	}

	private void Grid_ControlUserDeletedRow(object sender, DataGridViewRowEventArgs e)
	{
		DataGridView dg = (DataGridView)sender;
		if (dg.CurrentCell != null)
		{
			dg.Rows[dg.CurrentCell.RowIndex].Selected = true;
		}
		AfterUserDeletedRowEvent(sender, e);
	}

	public virtual void AfterUserDeletedRowEvent(object sender, DataGridViewRowEventArgs e)
	{
	}

	private void Grid_ControlUserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
	{
		_ = (DataGridView)sender;
		AfterUserDeletingRowEvent(sender, e);
	}

	public virtual void AfterUserDeletingRowEvent(object sender, DataGridViewRowCancelEventArgs e)
	{
	}

	private void Grid_ControlKeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			Convert.ToString(((DataGridView)sender).Name);
			DataGridView getGridControl = (DataGridView)sender;
			if (e.KeyData == Keys.F1)
			{
				Grid_F1KeyDown(sender, e);
			}
			if (getGridControl.Rows[getGridControl.CurrentCell.RowIndex].Selected)
			{
				if (e.KeyCode.Equals(Keys.Up))
				{
					moveUp(sender);
					e.Handled = true;
				}
				if (e.KeyCode.Equals(Keys.Down))
				{
					moveDown(sender);
					e.Handled = true;
				}
			}
			if (e.KeyCode != Keys.Return)
			{
				return;
			}
			int col = getGridControl.CurrentCell.ColumnIndex;
			int row = getGridControl.CurrentCell.RowIndex;
			int getColumn = GridHelper.Grid_GetColumnCount(getGridControl);
			if (col < getColumn)
			{
				col++;
				getGridControl.CurrentCell = getGridControl[col, row];
			}
			else if (col == getColumn)
			{
				col = 1;
				row++;
				if (row == getGridControl.RowCount)
				{
					getGridControl.Rows.Add();
				}
				getGridControl.CurrentCell = getGridControl[col, row];
			}
			e.Handled = true;
		}
		catch (Exception)
		{
		}
	}

	public virtual void Grid_F1KeyDown(object sender, KeyEventArgs e)
	{
	}

	public virtual void Grid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
	}

	public virtual void Grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
	{
	}

	public virtual void Grid_ControlListCellClick(object sender, DataGridViewCellEventArgs e)
	{
	}

	private void moveUp(object sender)
	{
		DataGridView dg = (DataGridView)sender;
		if (dg.RowCount <= 0 || dg.SelectedRows.Count <= 0)
		{
			return;
		}
		_ = dg.Rows.Count;
		int index = dg.SelectedCells[0].OwningRow.Index;
		if (index != 0)
		{
			DataGridViewRowCollection rows = dg.Rows;
			DataGridViewRow prevRow = rows[index - 1];
			rows.Remove(prevRow);
			prevRow.Frozen = false;
			rows.Insert(index, prevRow);
			dg.ClearSelection();
			dg.Rows[index - 1].Selected = true;
			if (dg.Columns.Contains("LineSno"))
			{
				dg["LineSno", index].Value = index + 1;
				dg["LineSno", index - 1].Value = index;
			}
			if (dg.Columns.Contains("ColSeq"))
			{
				dg["ColSeq", index].Value = index + 1;
				dg["ColSeq", index - 1].Value = index;
			}
			if (dg.Columns.Contains("FieldSeqNo"))
			{
				dg["FieldSeqNo", index].Value = index + 1;
				dg["FieldSeqNo", index - 1].Value = index;
			}
		}
	}

	private void moveDown(object sender)
	{
		DataGridView dg = (DataGridView)sender;
		if (dg.RowCount <= 0 || dg.SelectedRows.Count <= 0)
		{
			return;
		}
		int rowCount = dg.Rows.Count;
		int index = dg.SelectedCells[0].OwningRow.Index;
		if (index != rowCount - 2)
		{
			DataGridViewRowCollection rows = dg.Rows;
			DataGridViewRow nextRow = rows[index + 1];
			rows.Remove(nextRow);
			nextRow.Frozen = false;
			rows.Insert(index, nextRow);
			dg.ClearSelection();
			dg.Rows[index + 1].Selected = true;
			if (dg.Columns.Contains("LineSno"))
			{
				dg["LineSno", index].Value = index + 1;
				dg["LineSno", index + 1].Value = index + 2;
			}
			if (dg.Columns.Contains("ColSeq"))
			{
				dg["ColSeq", index].Value = index + 1;
				dg["ColSeq", index + 1].Value = index + 2;
			}
			if (dg.Columns.Contains("FieldSeqNo"))
			{
				dg["FieldSeqNo", index].Value = index + 1;
				dg["FieldSeqNo", index + 1].Value = index + 2;
			}
		}
	}

	private void Grid_List_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		if (e.RowIndex >= 0 && (int)CurrentActivity != 6 && (int)CurrentActivity != 1)
		{
			_ = (DataGridView)sender;
			AfterListGridCellClickEvent(sender, e);
		}
	}

	public virtual void AfterListGridCellClickEvent(object sender, DataGridViewCellEventArgs e)
	{
	}

	private void Grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
	{
		DataGridView dg = (DataGridView)sender;
		if (e.RowIndex >= 0 && dg[e.ColumnIndex, e.RowIndex].ValueType == typeof(bool))
		{
			dg.BeginEdit(selectAll: true);
			dg.EndEdit();
		}
	}

	private void Grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			DataGridView getGridControl = (DataGridView)sender;
			int colIndx = getGridControl.CurrentCell.ColumnIndex;
			int rowIndx = Convert.ToInt32(getGridControl.CurrentCell.RowIndex);
			_ = getGridControl.Columns[colIndx].Name;
			if (getGridControl.Columns.Contains("LineSno") && getGridControl["LineSno", rowIndx].Value == null)
			{
				getGridControl["LineSno", rowIndx].Value = rowIndx + 1;
			}
			AfterGridCellEndEditEvent(sender, e);
		}
		catch
		{
		}
	}

	public virtual void AfterGridCellEndEditEvent(object sender, DataGridViewCellEventArgs e)
	{
	}

	private void Grid_ControlDataError(object sender, DataGridViewDataErrorEventArgs anError)
	{
		try
		{
			DataGridView obj = (DataGridView)sender;
			_ = anError.ColumnIndex;
			_ = anError.RowIndex;
			_ = obj.Columns[anError.ColumnIndex].Name;
		}
		catch
		{
		}
	}

	private void cmb_SelectedIndexChanged(object sender, EventArgs e)
	{
		ComboBox tb = (ComboBox)sender;
		if (tb != null)
		{
			DataGridViewComboBoxEditingControl s = (DataGridViewComboBoxEditingControl)sender;
			DataGridView editingControlDataGridView = s.EditingControlDataGridView;
			object strValue = ((tb.SelectedValue != null) ? tb.SelectedValue.ToString() : null);
			if (tb.SelectedText != null)
			{
				tb.SelectedText.ToString();
			}
			editingControlDataGridView[tb.ValueMember, s.EditingControlRowIndex].Value = strValue;
		}
	}

	private void cmb_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
		ComboBox tb = (ComboBox)sender;
		if (tb == null)
		{
			return;
		}
		DataGridViewComboBoxEditingControl s = (DataGridViewComboBoxEditingControl)sender;
		DataGridView getGridControl = s.EditingControlDataGridView;
		if (e.KeyData == Keys.Escape)
		{
			object getGridText = getGridControl[tb.DisplayMember, s.EditingControlRowIndex].Value;
			object getGridValue = getGridControl[tb.ValueMember, s.EditingControlRowIndex].Value;
			if (getGridText == null)
			{
				getGridControl[tb.ValueMember, s.EditingControlRowIndex].Value = string.Empty;
			}
			else
			{
				getGridControl[tb.ValueMember, s.EditingControlRowIndex].Value = getGridValue.ToString();
			}
		}
	}

	public virtual void BeforeLoadControl()
	{
	}

	public virtual void AfterLoadControl()
	{
	}

	public void ResetControl()
	{
		try
		{
			BeforeLoadControl();
			btnDelete.Enabled = false;
			btnSave.Enabled = false;
			btnOpen.Enabled = true;
			btnNew.Enabled = true;
			btnPreview.Enabled = false;
			btnReset.Enabled = false;
			FormHelper.ControlStatus(this, status: false);
			FormHelper.ControlReset(this);
			ResetGrid();
			AfterLoadControl();
		}
		catch (Exception)
		{
			throw;
		}
	}

	public virtual void BeforeReset()
	{
	}

	public virtual void AfterReset()
	{
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
		try
		{
			BeforeReset();
			ResetControl();
			AfterReset();
		}
		catch (Exception)
		{
			throw;
		}
	}

	public virtual void DeleteClick()
	{
	}

	public virtual void AfterDelete()
	{
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			DeleteClick();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public virtual void SaveClick()
	{
	}

	public virtual void AfterSave()
	{
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			SaveClick();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public virtual void BeforeOpen()
	{
	}

	public virtual void AfterOpen()
	{
	}

	private void btnOpen_Click(object sender, EventArgs e)
	{
		try
		{
			BeforeOpen();
			FormHelper.ControlStatus(this, status: true);
			btnNew.Enabled = false;
			btnOpen.Enabled = false;
			btnSave.Enabled = true;
			btnPreview.Enabled = true;
			btnDelete.Enabled = true;
			btnReset.Enabled = true;
			focusControl.Focus();
			AfterOpen();
		}
		catch (Exception)
		{
			throw;
		}
	}

	public virtual void BeforeNew()
	{
	}

	public virtual void AfterNew()
	{
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			BeforeNew();
			FormHelper.ControlStatus(this, status: true);
			btnNew.Enabled = false;
			btnOpen.Enabled = false;
			btnSave.Enabled = true;
			btnPreview.Enabled = false;
			btnDelete.Enabled = false;
			btnReset.Enabled = true;
			focusControl.Focus();
			AfterNew();
			CurrentActivity = (ActionMode)1;
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void btnClose_Click(object sender, EventArgs e)
	{
		Close();
	}

	public virtual void txt_AfterKeyDown(object sender)
	{
	}

	public virtual void txt_AfterEnterKey(object sender)
	{
	}

	public virtual void txt_PreviewKeyDown1(object sender, PreviewKeyDownEventArgs e)
	{
	}

	public virtual void txt_TextChanged(object sender, EventArgs e)
	{
	}

	public virtual void mtb_AfterEnterKey(object sender)
	{
	}

	public void mtb_Leave(object sender, EventArgs e)
	{
		try
		{
			MaskedTextBox ctl = (MaskedTextBox)sender;
			BeginInvoke((MethodInvoker)delegate
			{
				ctl.DeselectAll();
			});
		}
		catch (Exception)
		{
			throw;
		}
	}

	public void mtb_Enter(object sender, EventArgs e)
	{
		try
		{
			MaskedTextBox ctl = (MaskedTextBox)sender;
			BeginInvoke((MethodInvoker)delegate
			{
				ctl.SelectAll();
			});
		}
		catch (Exception)
		{
			throw;
		}
	}

	public void txt_Enter(object sender, EventArgs e)
	{
		try
		{
			((TextBox)sender).SelectAll();
		}
		catch (Exception)
		{
			throw;
		}
	}

	public void txt_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
		try
		{
			if (e.KeyCode == Keys.Return)
			{
				txt_AfterEnterKey(sender);
				SendKeys.Send("{TAB}");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void mtb_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
		try
		{
			if (e.KeyCode == Keys.Return)
			{
				mtb_AfterEnterKey(sender);
				SendKeys.Send("{TAB}");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}
}

}
