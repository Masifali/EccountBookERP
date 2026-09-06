using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Export;

public class GdContainerBreakUp : Form
{
	public int GdIdFromBreakUp;

	private int rowIndexDetail = -1;

	private DataTable dtGdContainerBreakUp = new DataTable();

	private Rightsobjects formrights = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private ToolStrip toolStrip4;

	private ToolStripButton btnNew;

	private ToolStripButton btnsave;

	private ToolStripButton btnupdate;

	private Panel panel1;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label23;

	internal GridEX grdGdBreakUp;

	private Panel panel30;

	private TextBox txtContainerNo;

	private Label label121;

	private TextBox txtNetWeight;

	private Label label116;

	private TextBox txtNoOfBags;

	private Label label110;

	internal UltraCombo CmbGdNo;

	private Label label105;

	private TextBox txtContainerSize;

	private Label label1;

	private Button btnAdd;

	private Button btnUpdateDetail;

	private Button btnCancelDetail;

	public GdContainerBreakUp(UserAccount PUser)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = PUser;
	}

	private void GdBreakUpManual_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1100, 700);
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			ctrlGrdBar1.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			dtGdContainerBreakUp.Columns.Add("Id", typeof(int));
			dtGdContainerBreakUp.Columns.Add("GdId", typeof(int));
			dtGdContainerBreakUp.Columns.Add("GdNo", typeof(string));
			dtGdContainerBreakUp.Columns.Add("ExImInvoiceId", typeof(int));
			dtGdContainerBreakUp.Columns.Add("InvoiceNo", typeof(string));
			dtGdContainerBreakUp.Columns.Add("ContainerNo", typeof(string));
			dtGdContainerBreakUp.Columns.Add("ContainerSize", typeof(string));
			dtGdContainerBreakUp.Columns.Add("NoOfBags", typeof(decimal));
			dtGdContainerBreakUp.Columns.Add("NetWeight", typeof(decimal));
			grdGdBreakUp.DataSource = dtGdContainerBreakUp;
			grdGdBreakUp.RetrieveStructure();
			grdSettingsGdBreakUp();
			GdNoBind();
			if (GdIdFromBreakUp > 0)
			{
				CmbGdNo.Value = GdIdFromBreakUp;
			}
			CmbGdNo.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GdNoBind()
	{
		try
		{
			DataTable dt = GDBreakUpHeader.getGdsForGdContainerBreakUp(UserAccount.CompanyId, GdIdFromBreakUp);
			if (dt.Rows.Count > 0)
			{
				InfragisticsHelper.BindAndRetainSelection(CmbGdNo, dt, "Id", "GDNO", "Gd No", AllColumns: true, Conversion.ToInt(CmbGdNo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
				((UltraGridBase)CmbGdNo).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
			else
			{
				((Control)(object)CmbGdNo).Text = string.Empty;
				((UltraGridBase)CmbGdNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Reset()
	{
		try
		{
			btnsave.Visible = true;
			btnupdate.Visible = false;
			GdIdFromBreakUp = 0;
			dtGdContainerBreakUp.Rows.Clear();
			grdGdBreakUp.DataSource = dtGdContainerBreakUp;
			((Control)(object)CmbGdNo).Text = string.Empty;
			((Control)(object)CmbGdNo).Enabled = true;
			txtContainerNo.Text = string.Empty;
			txtContainerSize.Text = string.Empty;
			txtNoOfBags.Clear();
			txtNetWeight.Clear();
			CmbGdNo.Focus();
			GdNoBind();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdGdBreakUp).Name);
			if (fs != null)
			{
				grdGdBreakUp.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool GdBreakUpFormValidation()
	{
		if (((UltraGridBase)CmbGdNo).ActiveRow == null || ((Control)(object)CmbGdNo).Text == string.Empty)
		{
			MessageBox.Show("GdNo field is required");
			CmbGdNo.Focus();
			return false;
		}
		if (txtContainerNo.Text == null || txtContainerNo.Text == string.Empty)
		{
			MessageBox.Show("Container No field is required");
			txtContainerNo.Focus();
			return false;
		}
		if (txtContainerSize.Text == null || txtContainerSize.Text == string.Empty)
		{
			MessageBox.Show("Container Size field is required");
			txtContainerSize.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtNoOfBags.Text.Trim()) <= 0.0)
		{
			MessageBox.Show("NoOfBags field is Required");
			txtNoOfBags.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtNetWeight.Text.Trim()) <= 0.0)
		{
			MessageBox.Show("Net Weight field is Required");
			txtNetWeight.Focus();
			return false;
		}
		return true;
	}

	private void grdGdBreakUp_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdGdBreakUp.CurrentRow;
			if (item != null)
			{
				rowIndexDetail = item.RowIndex;
				CmbGdNo.Value = Conversion.ToInt(item.Cells["GdId"].Value);
				txtContainerNo.Text = Conversion.ToString(item.Cells["ContainerNo"].Value);
				txtContainerSize.Text = Conversion.ToString(item.Cells["ContainerSize"].Value);
				txtNoOfBags.Text = Conversion.ToDouble(item.Cells["NoOfBags"].Value).ToString("#,##0.###");
				txtNetWeight.Text = Conversion.ToDouble(item.Cells["NetWeight"].Value).ToString("#,##0.###");
				btnAdd.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelDetail.Visible = true;
				CmbGdNo.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		try
		{
			if (grdGdBreakUp.RowCount == 0)
			{
				throw new Exception("Detail Grid Not found.Please Enter at Least one entry");
			}
			GdContainerBreakUp obj = new GdContainerBreakUp();
			if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				obj.GdContainerBreakUpList = new List<GdContainerBreakUp>();
				GridEXRow[] dataRows = grdGdBreakUp.GetDataRows();
				foreach (GridEXRow item in dataRows)
				{
					GdContainerBreakUp d = new GdContainerBreakUp();
					d.GdId = Conversion.ToInt(item.Cells["GdId"].Value);
					d.ExImInvoiceId = Conversion.ToInt(item.Cells["ExImInvoiceId"].Value);
					d.ContainerNo = Conversion.ToString(item.Cells["ContainerNo"].Value);
					d.ContainerSize = Conversion.ToString(item.Cells["ContainerSize"].Value);
					d.NoOfBags = Conversion.ToDecimal(item.Cells["NoOfBags"].Value);
					d.NetWeight = Conversion.ToDecimal(item.Cells["NetWeight"].Value);
					obj.GdContainerBreakUpList.Add(d);
				}
				GDBreakUpHeader.SaveGdContainerBreakUp(obj);
				MessageBox.Show("Save SuccessFully");
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			GdIdFromBreakUp = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (GdIdFromBreakUp == 0)
			{
				throw new Exception("RecId not found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdSettingsGdBreakUp()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "GdId", "ExImInvoiceId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdGdBreakUp, 2, 3, hiddenColumns);
			grdGdBreakUp.RootTable.Columns["GdNo"].Width = Constants.ExportConstants.InvoiceNoConstant + 50;
			grdGdBreakUp.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
			grdGdBreakUp.RootTable.Columns["ContainerNo"].Width = Constants.ExportConstants.ContainerNo;
			grdGdBreakUp.RootTable.Columns["ContainerSize"].Width = Constants.ExportConstants.UomConstant + 50;
			grdGdBreakUp.RootTable.Columns["NoOfBags"].Width = Constants.ExportConstants.QtyConstant + 20;
			grdGdBreakUp.RootTable.Columns["NetWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdGdBreakUp.RootTable.Columns["NoOfBags"].AggregateFunction = (AggregateFunction)2;
			grdGdBreakUp.RootTable.Columns["NoOfBags"].TextAlignment = (TextAlignment)3;
			grdGdBreakUp.RootTable.Columns["NoOfBags"].FormatString = "#,##0.###";
			grdGdBreakUp.RootTable.Columns["NoOfBags"].TotalFormatString = "#,##0.###";
			grdGdBreakUp.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdGdBreakUp.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdGdBreakUp.RootTable.Columns["NetWeight"].FormatString = "#,##0.###";
			grdGdBreakUp.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.###";
			GridEX_Helper.AddButton(grdGdBreakUp, "Delete", "X", 20);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFreight_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFobValue_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFreight_TextChanged(object sender, EventArgs e)
	{
	}

	private void txtFobValue_TextChanged(object sender, EventArgs e)
	{
	}

	private void GdBreakUpManual_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 83;
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				Reset();
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.U)
			{
				btnupdate_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetGdBreakUpDetails()
	{
		try
		{
			rowIndexDetail = -1;
			txtContainerNo.Text = string.Empty;
			txtContainerSize.Text = string.Empty;
			txtNoOfBags.Text = string.Empty;
			txtNetWeight.Clear();
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbGdNo_Leave(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(CmbGdNo.Value);
			int GdIdFromGrid = ((grdGdBreakUp.RowCount > 0) ? Conversion.ToInt(grdGdBreakUp.GetRow(0).Cells["GdId"].Value) : 0);
			if (Id > 0 && GdIdFromGrid > 0 && Id != GdIdFromGrid && MessageBox.Show("Are you sure to load other gd data?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			if (GdIdFromGrid > 0)
			{
				dtGdContainerBreakUp.Rows.Clear();
			}
			DataTable dt = GDBreakUpHeader.getGdContainerBreakUpAgainstGd(UserAccount.CompanyId, Id);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtGdContainerBreakUp.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["GdId"], dt.Rows[i]["GDNO"], dt.Rows[i]["ExImInvoiceId"], dt.Rows[i]["InvoiceNo"], dt.Rows[i]["ContainerNo"], dt.Rows[i]["ContainerSize"], dt.Rows[i]["NoOfBags"], dt.Rows[i]["NetWeight"]);
				}
			}
			grdGdBreakUp.DataSource = dtGdContainerBreakUp;
			grdGdBreakUp.RetrieveStructure();
			grdSettingsGdBreakUp();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (!GdBreakUpFormValidation())
			{
				return;
			}
			if (dtGdContainerBreakUp.Rows.Count > 0)
			{
				for (int i = 0; i < dtGdContainerBreakUp.Rows.Count; i++)
				{
					if (Conversion.ToInt(dtGdContainerBreakUp.Rows[i]["GdId"]) != Conversion.ToInt(CmbGdNo.Value))
					{
						throw new Exception("You can only add data against one Gd in the grid");
					}
				}
			}
			dtGdContainerBreakUp.Rows.Add(0, CmbGdNo.Value, ((Control)(object)CmbGdNo).Text.Trim(), ((UltraDropDownBase)CmbGdNo).SelectedRow.Cells["ExImInvoiceId"].Value, ((UltraDropDownBase)CmbGdNo).SelectedRow.Cells["InvoiceNo"].Value, Conversion.ToString((object)txtContainerNo.Text), Conversion.ToString((object)txtContainerSize.Text), Conversion.ToDouble((object)txtNoOfBags.Text), Conversion.ToDouble((object)txtNetWeight.Text));
			grdGdBreakUp.DataSource = dtGdContainerBreakUp;
			grdGdBreakUp.RetrieveStructure();
			grdSettingsGdBreakUp();
			ResetGdBreakUpDetails();
			((Control)(object)CmbGdNo).Enabled = false;
			txtContainerNo.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (!GdBreakUpFormValidation())
			{
				return;
			}
			if (dtGdContainerBreakUp.Rows.Count > 0)
			{
				for (int i = 0; i < dtGdContainerBreakUp.Rows.Count; i++)
				{
					if (i != rowIndexDetail && Conversion.ToInt(dtGdContainerBreakUp.Rows[i]["GdId"]) != Conversion.ToInt(CmbGdNo.Value))
					{
						throw new Exception("You can only add data against one Gd in the grid");
					}
				}
			}
			dtGdContainerBreakUp.Rows[rowIndexDetail]["GdId"] = CmbGdNo.Value;
			dtGdContainerBreakUp.Rows[rowIndexDetail]["GdNo"] = ((Control)(object)CmbGdNo).Text;
			dtGdContainerBreakUp.Rows[rowIndexDetail]["ExImInvoiceId"] = ((UltraDropDownBase)CmbGdNo).SelectedRow.Cells["ExImInvoiceId"].Value;
			dtGdContainerBreakUp.Rows[rowIndexDetail]["InvoiceNo"] = ((UltraDropDownBase)CmbGdNo).SelectedRow.Cells["InvoiceNo"].Value;
			dtGdContainerBreakUp.Rows[rowIndexDetail]["ContainerNo"] = txtContainerNo.Text.Trim();
			dtGdContainerBreakUp.Rows[rowIndexDetail]["ContainerSize"] = txtContainerSize.Text.Trim();
			dtGdContainerBreakUp.Rows[rowIndexDetail]["NoOfBags"] = Conversion.ToDecimal((object)txtNoOfBags.Text);
			dtGdContainerBreakUp.Rows[rowIndexDetail]["NetWeight"] = Conversion.ToDecimal((object)txtNetWeight.Text);
			grdGdBreakUp.DataSource = dtGdContainerBreakUp;
			ResetGdBreakUpDetails();
			txtContainerNo.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelDetail_Click(object sender, EventArgs e)
	{
		try
		{
			ResetGdBreakUpDetails();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGdBreakUp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdGdBreakUp.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					r.Delete();
				}
				if (grdGdBreakUp.RowCount == 0)
				{
					((Control)(object)CmbGdNo).Enabled = true;
				}
			}
			grdGdBreakUp.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Expected O, but got Unknown
		GridEXLayout grdGdBreakUp_DesignTimeLayout = new GridEXLayout();
		Appearance appearance541 = new Appearance();
		Appearance appearance542 = new Appearance();
		Appearance appearance543 = new Appearance();
		Appearance appearance544 = new Appearance();
		Appearance appearance545 = new Appearance();
		Appearance appearance546 = new Appearance();
		Appearance appearance547 = new Appearance();
		Appearance appearance548 = new Appearance();
		Appearance appearance549 = new Appearance();
		Appearance appearance550 = new Appearance();
		Appearance appearance551 = new Appearance();
		Appearance appearance552 = new Appearance();
		this.grdGdBreakUp = new GridEX();
		this.panel30 = new System.Windows.Forms.Panel();
		this.btnAdd = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.txtContainerSize = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtContainerNo = new System.Windows.Forms.TextBox();
		this.label121 = new System.Windows.Forms.Label();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.label116 = new System.Windows.Forms.Label();
		this.txtNoOfBags = new System.Windows.Forms.TextBox();
		this.label110 = new System.Windows.Forms.Label();
		this.CmbGdNo = new UltraCombo();
		this.label105 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label23 = new System.Windows.Forms.Label();
		this.toolStrip4 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		((System.ComponentModel.ISupportInitialize)this.grdGdBreakUp).BeginInit();
		this.panel30.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbGdNo).BeginInit();
		this.panel1.SuspendLayout();
		this.toolStrip4.SuspendLayout();
		base.SuspendLayout();
		this.grdGdBreakUp.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdGdBreakUp).BackColor = System.Drawing.Color.White;
		grdGdBreakUp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGdBreakUp.DesignTimeLayout = grdGdBreakUp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdGdBreakUp.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGdBreakUp.GroupByBoxVisible = false;
		this.grdGdBreakUp.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).Location = new System.Drawing.Point(0, 110);
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).Name = "grdGdBreakUp";
		((ScrollableBase)this.grdGdBreakUp).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdGdBreakUp).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdGdBreakUp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).Size = new System.Drawing.Size(1019, 388);
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).TabIndex = 605;
		this.grdGdBreakUp.TotalRow = (InheritableBoolean)1;
		this.grdGdBreakUp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGdBreakUp).VisualStyle = (VisualStyle)4;
		this.grdGdBreakUp.ColumnButtonClick += new ColumnActionEventHandler(grdGdBreakUp_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).DoubleClick += new System.EventHandler(grdGdBreakUp_DoubleClick);
		this.panel30.Controls.Add(this.btnAdd);
		this.panel30.Controls.Add(this.btnUpdateDetail);
		this.panel30.Controls.Add(this.btnCancelDetail);
		this.panel30.Controls.Add(this.txtContainerSize);
		this.panel30.Controls.Add(this.label1);
		this.panel30.Controls.Add(this.txtContainerNo);
		this.panel30.Controls.Add(this.label121);
		this.panel30.Controls.Add(this.txtNetWeight);
		this.panel30.Controls.Add(this.label116);
		this.panel30.Controls.Add(this.txtNoOfBags);
		this.panel30.Controls.Add(this.label110);
		this.panel30.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGdNo);
		this.panel30.Controls.Add(this.label105);
		this.panel30.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel30.Location = new System.Drawing.Point(0, 55);
		this.panel30.Name = "panel30";
		this.panel30.Size = new System.Drawing.Size(1019, 55);
		this.panel30.TabIndex = 606;
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatAppearance.BorderSize = 0;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(697, 27);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(31, 23);
		this.btnAdd.TabIndex = 5;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(697, 27);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(63, 23);
		this.btnUpdateDetail.TabIndex = 6;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.btnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetail.FlatAppearance.BorderSize = 0;
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(697, 3);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(63, 23);
		this.btnCancelDetail.TabIndex = 7;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Visible = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.txtContainerSize.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtContainerSize.Location = new System.Drawing.Point(328, 27);
		this.txtContainerSize.Name = "txtContainerSize";
		this.txtContainerSize.Size = new System.Drawing.Size(135, 23);
		this.txtContainerSize.TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(328, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(83, 15);
		this.label1.TabIndex = 616;
		this.label1.Text = "Container Size";
		this.txtContainerNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtContainerNo.Location = new System.Drawing.Point(192, 27);
		this.txtContainerNo.Name = "txtContainerNo";
		this.txtContainerNo.Size = new System.Drawing.Size(135, 23);
		this.txtContainerNo.TabIndex = 1;
		this.label121.AutoSize = true;
		this.label121.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label121.Location = new System.Drawing.Point(192, 6);
		this.label121.Name = "label121";
		this.label121.Size = new System.Drawing.Size(77, 15);
		this.label121.TabIndex = 614;
		this.label121.Text = "Container No";
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeight.Location = new System.Drawing.Point(580, 27);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.Size = new System.Drawing.Size(115, 23);
		this.txtNetWeight.TabIndex = 4;
		this.txtNetWeight.Text = "0";
		this.txtNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetWeight.TextChanged += new System.EventHandler(txtFobValue_TextChanged);
		this.txtNetWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFobValue_KeyPress);
		this.label116.AutoSize = true;
		this.label116.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label116.Location = new System.Drawing.Point(580, 6);
		this.label116.Name = "label116";
		this.label116.Size = new System.Drawing.Size(68, 15);
		this.label116.TabIndex = 604;
		this.label116.Text = "Net Weight";
		this.txtNoOfBags.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfBags.Location = new System.Drawing.Point(464, 27);
		this.txtNoOfBags.Name = "txtNoOfBags";
		this.txtNoOfBags.Size = new System.Drawing.Size(115, 23);
		this.txtNoOfBags.TabIndex = 3;
		this.txtNoOfBags.Text = "0";
		this.txtNoOfBags.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNoOfBags.TextChanged += new System.EventHandler(txtFreight_TextChanged);
		this.txtNoOfBags.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFreight_KeyPress);
		this.label110.AutoSize = true;
		this.label110.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label110.Location = new System.Drawing.Point(464, 6);
		this.label110.Name = "label110";
		this.label110.Size = new System.Drawing.Size(61, 15);
		this.label110.TabIndex = 602;
		this.label110.Text = "NoOfBags";
		this.CmbGdNo.AllowNull = (DefaultableBoolean)1;
		this.CmbGdNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGdNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGdNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance541).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance541).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance541;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance542).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance542).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance542).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance542).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGdNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance542;
		((AppearanceBase)appearance543).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance543;
		((SpecialBoxBase)((UltraGridBase)this.CmbGdNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance544).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance544).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance544).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance544).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance544;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance545).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance545).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance545;
		((AppearanceBase)appearance546).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance546).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance546;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance547).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance547;
		((AppearanceBase)appearance548).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance548).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance548;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance549).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance549).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance549).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance549).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance549).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance549;
		((AppearanceBase)appearance550).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance550;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance551).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance551).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance551;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance552).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance552;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGdNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbGdNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGdNo).Location = new System.Drawing.Point(7, 25);
		((System.Windows.Forms.Control)(object)this.CmbGdNo).Name = "CmbGdNo";
		((System.Windows.Forms.Control)(object)this.CmbGdNo).Size = new System.Drawing.Size(184, 26);
		((System.Windows.Forms.Control)(object)this.CmbGdNo).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbGdNo).Leave += new System.EventHandler(CmbGdNo_Leave);
		this.label105.AutoSize = true;
		this.label105.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label105.Location = new System.Drawing.Point(7, 6);
		this.label105.Name = "label105";
		this.label105.Size = new System.Drawing.Size(41, 15);
		this.label105.TabIndex = 598;
		this.label105.Text = "Gd No";
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.ctrlGrdBar1);
		this.panel1.Controls.Add(this.label23);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 27);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1019, 28);
		this.panel1.TabIndex = 6;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(977, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdGdBreakUp;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(40, 23);
		this.ctrlGrdBar1.TabIndex = 66;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(176, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "GD Container Break ";
		this.toolStrip4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNew, this.btnsave, this.btnupdate });
		this.toolStrip4.Location = new System.Drawing.Point(0, 0);
		this.toolStrip4.Name = "toolStrip4";
		this.toolStrip4.Size = new System.Drawing.Size(1019, 27);
		this.toolStrip4.TabIndex = 5;
		this.toolStrip4.Text = "toolStrip4";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1019, 498);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdGdBreakUp);
		base.Controls.Add(this.panel30);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.toolStrip4);
		base.KeyPreview = true;
		base.Name = "GdContainerBreakUp";
		this.Text = "GD Container BreakUp";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(GdBreakUpManual_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(GdBreakUpManual_KeyDown);
		((System.ComponentModel.ISupportInitialize)this.grdGdBreakUp).EndInit();
		this.panel30.ResumeLayout(false);
		this.panel30.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbGdNo).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip4.ResumeLayout(false);
		this.toolStrip4.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
