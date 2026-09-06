using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Mfg;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Mfg;
using Architecture.WinApp.Common;
using Architecture.WinApp.Inventory_Definition;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Mfg
{

public class WorkStationOperation : Form
{
	private UserAccount UserAccount = new UserAccount();

	private int RecId;

	private DropDownBind DDL = new DropDownBind();

	private IContainer components;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnShortcutkeys;

	private ToolStripButton btnUpdate;

	private Panel panel4;

	private Label label15;

	private Panel panel1;

	private Label label3;

	private Panel panel2;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdDetail;

	private Label label1;

	internal UltraCombo cmbworkstationname;

	internal UltraCombo cmboperationname;

	private ToolStripButton btnRefresh;

	private ToolStripButton BtnOperation;

	private ToolStripButton BtnProductionWorkstation;

	private TextBox txtOpHoursPerDay;

	private Label label14;

	private Button BtnSaveII;

	internal GridEX grdII;

	public WorkStationOperation(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void WorkStationOperation_Load(object sender, EventArgs e)
	{
		try
		{
			base.WindowState = FormWindowState.Normal;
			base.Size = new Size(725, 680);
			base.StartPosition = FormStartPosition.Manual;
			if (base.Parent != null)
			{
				int centerX = base.Parent.Left + (base.Parent.Width - base.Width) / 2;
				int centerY = base.Parent.Top + (base.Parent.Height - base.Height) / 2;
				base.Location = new Point(centerX, centerY);
			}
			else
			{
				base.StartPosition = FormStartPosition.CenterParent;
			}
			OeraptionNamefill();
			StationNamefill();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			BtnSaveII.Text = "Save";
			txtOpHoursPerDay.Visible = true;
			txtOpHoursPerDay.Enabled = true;
			cmbworkstationname.Focus();
			FormHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OeraptionNamefill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(cmboperationname.Value);
			DataTable dataTable = ProductionOperation.FormHistory(new ProductionOperation
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchId = UserAccount.BranchesId
			});
			if (dataTable.Rows.Count > 0)
			{
				DDL.BindDDLNew(dataTable, cmboperationname, "Id", "OperationName", "Operation Name", true);
				((UltraGridBase)cmboperationname).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (Id > 0)
			{
				if (dataTable.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					cmboperationname.Value = Id;
				}
				else
				{
					((Control)(object)cmboperationname).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StationNamefill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(cmbworkstationname.Value);
			DataTable dataTable = ProductionWorkStation.FormHistory(new ProductionWorkStation
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchId = UserAccount.BranchesId
			});
			if (dataTable.Rows.Count > 0)
			{
				DDL.BindDDLNew(dataTable, cmbworkstationname, "Id", "WorkStationName", "Station Name", true);
				((UltraGridBase)cmbworkstationname).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (id > 0)
			{
				if (dataTable.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					cmbworkstationname.Value = id;
				}
				else
				{
					((Control)(object)cmbworkstationname).Text = string.Empty;
				}
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
			RecId = 0;
			BtnSaveII.Text = "Save";
			((Control)(object)cmboperationname).Text = string.Empty;
			((Control)(object)cmbworkstationname).Text = string.Empty;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			cmbworkstationname.Focus();
			FormHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			OeraptionNamefill();
			StationNamefill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WorkStationOperation_FormClosed(object sender, FormClosedEventArgs e)
	{
		try
		{
			HistoryStack.Remove(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((Control)(object)cmboperationname).Text == string.Empty || Conversion.ToInt(cmboperationname.Value) == 0)
		{
			MessageBox.Show("Operation Name Field Is Required");
			cmboperationname.Focus();
			return false;
		}
		if (((Control)(object)cmbworkstationname).Text == string.Empty || Conversion.ToInt(cmbworkstationname.Value) == 0)
		{
			MessageBox.Show("Work Station Name Field Is Required");
			cmbworkstationname.Focus();
			return false;
		}
		return true;
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			WorkStationOperation obj = new WorkStationOperation();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				obj.WorkStationOperationId = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchId = UserAccount.BranchesId;
			obj.ProductionOperationId = Conversion.ToInt(cmboperationname.Value);
			obj.ProductionWorkStationId = Conversion.ToInt(cmbworkstationname.Value);
			obj.OpHoursPerDay = Conversion.ToDecimal((object)txtOpHoursPerDay.Text);
			WorkStationOperation.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully");
			}
			else
			{
				MessageBox.Show("Record Save Successfully");
			}
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("RecId not found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnSaveII_Click(object sender, EventArgs e)
	{
		try
		{
			if (BtnSaveII.Text == "Save")
			{
				btnsave_Click(null, null);
			}
			else if (BtnSaveII.Text == "Update")
			{
				btnUpdate_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FormHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			DataTable dt = WorkStationOperation.FormHistory(new WorkStationOperation
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchId = UserAccount.BranchesId
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dataII = CreateGroupedDataTable(dt, "OperationName", "SerialNo");
				grdII.DataSource = dataII;
				grdII.RetrieveStructure();
				GridSetting(grdII, "OperationName");
				DataTable data = CreateGroupedDataTable(dt, "WorkStationName", "SerialNo");
				grdDetail.DataSource = data;
				grdDetail.RetrieveStructure();
				GridSetting(grdDetail, "WorkStationName");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("An error occurred while loading form history: " + ex.Message);
		}
	}

	private DataTable CreateGroupedDataTable(DataTable sourceTable, string groupByColumn, string serialColumn)
	{
		DataTable groupedTable = new DataTable();
		groupedTable.Columns.Add("Id", typeof(int));
		groupedTable.Columns.Add(serialColumn, typeof(int));
		groupedTable.Columns.Add("WorkStationName", typeof(string));
		groupedTable.Columns.Add("OperationName", typeof(string));
		groupedTable.Columns.Add("OpHoursPerDay", typeof(decimal));
		foreach (IGrouping<string, DataRow> item in from row in sourceTable.AsEnumerable()
			group row by row.Field<string>(groupByColumn))
		{
			int serialNo = 1;
			foreach (DataRow row2 in item)
			{
				groupedTable.Rows.Add(row2.Field<int>("Id"), serialNo++, row2.Field<string>("WorkStationName"), row2.Field<string>("OperationName"), row2.Field<decimal>("OpHoursPerDay"));
			}
		}
		return groupedTable;
	}

	private void GridSetting(GridEX grid, string groupByColumn)
	{
		try
		{
			grid.RootTable.Columns["Id"].Visible = false;
			grid.RootTable.Groups.Add(groupByColumn);
			grid.RootTable.Columns[groupByColumn].Visible = false;
			CommonServices.GridWrappingAndColumnSettings(grid, 2);
			grid.RootTable.Columns["SerialNo"].Width = 60;
			grid.RootTable.Columns["WorkStationName"].Width = 180;
			grid.RootTable.Columns["OperationName"].Width = 180;
			grid.RootTable.Columns["OpHoursPerDay"].Width = 70;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show("An error occurred while configuring the grid: " + ex.Message);
		}
	}

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			BtnSaveII.Text = "Update";
			RecId = Conversion.ToInt(grdDetail.CurrentRow.Cells["Id"].Value);
			((Control)(object)cmboperationname).Text = Conversion.ToString(grdDetail.CurrentRow.Cells["OperationName"].Value);
			((Control)(object)cmbworkstationname).Text = Conversion.ToString(grdDetail.CurrentRow.Cells["WorkStationName"].Value);
			txtOpHoursPerDay.Text = Conversion.ToString(grdDetail.CurrentRow.Cells["OpHoursPerDay"].Value);
			cmboperationname.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdII_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			BtnSaveII.Text = "Update";
			RecId = Conversion.ToInt(grdII.CurrentRow.Cells["Id"].Value);
			((Control)(object)cmboperationname).Text = Conversion.ToString(grdII.CurrentRow.Cells["OperationName"].Value);
			((Control)(object)cmbworkstationname).Text = Conversion.ToString(grdII.CurrentRow.Cells["WorkStationName"].Value);
			txtOpHoursPerDay.Text = Conversion.ToString(grdII.CurrentRow.Cells["OpHoursPerDay"].Value);
			cmboperationname.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WorkStationOperation_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			else if (btnsave.Visible && btnsave.Enabled)
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnsave_Click(null, null);
				}
			}
			else if (btnUpdate.Visible && btnUpdate.Enabled)
			{
				if (e.Control && e.KeyCode == Keys.U)
				{
					btnUpdate_Click(null, null);
				}
			}
			else if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			else if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.F5)
			{
				cmboperationname.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.F1)
			{
				BtnOperation_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.F2)
			{
				BtnProductionWorkstation_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Up)
			{
				cmboperationname.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdDetail).Focus();
			}
			else if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F1", "For Open On Work Station Define Form");
			dt.Rows.Add("Ctrl+F2", "For Open On Operation Define form");
			dt.Rows.Add("Ctrl+F5", "For Focus On Work Station Name");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Work Station Name");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortcutkeys_Click(object sender, EventArgs e)
	{
		try
		{
			MakeShortCutKeys();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetail).Name);
			if (fs != null)
			{
				grdDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void BtnOperation_Click(object sender, EventArgs e)
	{
		try
		{
			new ProductionOperation(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnProductionWorkstation_Click(object sender, EventArgs e)
	{
		try
		{
			InvDeffrmWarehouse invDeffrmWarehouse = new InvDeffrmWarehouse(UserAccount);
			invDeffrmWarehouse.CmbWarehouseType.Value = 4;
			invDeffrmWarehouse.Show();
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Mfg.WorkStationOperation));
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdII_DesignTimeLayout = new GridEXLayout();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnShortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.BtnOperation = new System.Windows.Forms.ToolStripButton();
		this.BtnProductionWorkstation = new System.Windows.Forms.ToolStripButton();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.txtOpHoursPerDay = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.cmbworkstationname = new UltraCombo();
		this.cmboperationname = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdDetail = new GridEX();
		this.BtnSaveII = new System.Windows.Forms.Button();
		this.grdII = new GridEX();
		this.toolStrip1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbworkstationname).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmboperationname).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdII).BeginInit();
		base.SuspendLayout();
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnnew, this.btnRefresh, this.btnsave, this.btnShortcutkeys, this.btnUpdate, this.BtnOperation, this.BtnProductionWorkstation });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(709, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnShortcutkeys.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortcutkeys.Image = (System.Drawing.Image)resources.GetObject("btnShortcutkeys.Image");
		this.btnShortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutkeys.Name = "btnShortcutkeys";
		this.btnShortcutkeys.Size = new System.Drawing.Size(106, 24);
		this.btnShortcutkeys.Text = "ShortCut Keys";
		this.btnShortcutkeys.Click += new System.EventHandler(btnShortcutkeys_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.BtnOperation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.BtnOperation.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnOperation.Image = (System.Drawing.Image)resources.GetObject("BtnOperation.Image");
		this.BtnOperation.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnOperation.Name = "BtnOperation";
		this.BtnOperation.Size = new System.Drawing.Size(128, 24);
		this.BtnOperation.Text = "Define Operations";
		this.BtnOperation.Click += new System.EventHandler(BtnOperation_Click);
		this.BtnProductionWorkstation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.BtnProductionWorkstation.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnProductionWorkstation.Image = (System.Drawing.Image)resources.GetObject("BtnProductionWorkstation.Image");
		this.BtnProductionWorkstation.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnProductionWorkstation.Name = "BtnProductionWorkstation";
		this.BtnProductionWorkstation.Size = new System.Drawing.Size(149, 24);
		this.BtnProductionWorkstation.Text = "Define Work Stations";
		this.BtnProductionWorkstation.Visible = false;
		this.BtnProductionWorkstation.Click += new System.EventHandler(BtnProductionWorkstation_Click);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label15);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 27);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(709, 28);
		this.panel4.TabIndex = 8;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(5, 4);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(202, 19);
		this.label15.TabIndex = 1;
		this.label15.Text = "Work Station Operation";
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.BtnSaveII);
		this.panel1.Controls.Add(this.txtOpHoursPerDay);
		this.panel1.Controls.Add(this.label14);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbworkstationname);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.cmboperationname);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Controls.Add(this.label3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 55);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(709, 58);
		this.panel1.TabIndex = 0;
		this.txtOpHoursPerDay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOpHoursPerDay.Enabled = false;
		this.txtOpHoursPerDay.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOpHoursPerDay.Location = new System.Drawing.Point(439, 26);
		this.txtOpHoursPerDay.Name = "txtOpHoursPerDay";
		this.txtOpHoursPerDay.Size = new System.Drawing.Size(185, 23);
		this.txtOpHoursPerDay.TabIndex = 2;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(439, 6);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(65, 15);
		this.label14.TabIndex = 90;
		this.label14.Text = "Hours/Day";
		this.cmbworkstationname.AllowNull = (DefaultableBoolean)1;
		this.cmbworkstationname.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbworkstationname.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbworkstationname.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbworkstationname).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbworkstationname).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbworkstationname).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbworkstationname).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbworkstationname).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbworkstationname).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbworkstationname).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbworkstationname).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbworkstationname).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbworkstationname).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbworkstationname).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbworkstationname).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbworkstationname).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbworkstationname).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance12).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbworkstationname).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbworkstationname).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbworkstationname).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbworkstationname).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbworkstationname).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbworkstationname).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbworkstationname).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbworkstationname).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbworkstationname).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbworkstationname).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbworkstationname).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbworkstationname).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbworkstationname).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbworkstationname).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbworkstationname.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbworkstationname).Location = new System.Drawing.Point(28, 24);
		((System.Windows.Forms.Control)(object)this.cmbworkstationname).Name = "cmbworkstationname";
		((System.Windows.Forms.Control)(object)this.cmbworkstationname).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbworkstationname).TabIndex = 0;
		this.cmboperationname.AllowNull = (DefaultableBoolean)1;
		this.cmboperationname.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmboperationname.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmboperationname.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmboperationname).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmboperationname).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmboperationname).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmboperationname).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmboperationname).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.cmboperationname).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmboperationname).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmboperationname).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmboperationname).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmboperationname).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmboperationname).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmboperationname).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmboperationname).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmboperationname).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmboperationname).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmboperationname).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmboperationname).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmboperationname).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmboperationname).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmboperationname).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmboperationname).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmboperationname).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmboperationname).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmboperationname).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmboperationname).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmboperationname).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmboperationname).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmboperationname).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmboperationname.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmboperationname).Location = new System.Drawing.Point(235, 24);
		((System.Windows.Forms.Control)(object)this.cmboperationname).Name = "cmboperationname";
		((System.Windows.Forms.Control)(object)this.cmboperationname).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmboperationname).TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(235, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(95, 15);
		this.label1.TabIndex = 88;
		this.label1.Text = "Operation Name";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(25, 6);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(115, 15);
		this.label3.TabIndex = 86;
		this.label3.Text = " Work Station Name";
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 113);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(709, 29);
		this.panel2.TabIndex = 14;
		this.panel2.Visible = false;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(664, 0);
		this.ctrlGrdBar1.MyGrid = this.grdDetail;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(40, 26);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Left;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 142);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(350, 499);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 1;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		this.BtnSaveII.BackColor = System.Drawing.Color.Teal;
		this.BtnSaveII.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnSaveII.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSaveII.ForeColor = System.Drawing.Color.White;
		this.BtnSaveII.Location = new System.Drawing.Point(627, 26);
		this.BtnSaveII.Name = "BtnSaveII";
		this.BtnSaveII.Size = new System.Drawing.Size(75, 26);
		this.BtnSaveII.TabIndex = 3;
		this.BtnSaveII.Text = "Update";
		this.BtnSaveII.UseVisualStyleBackColor = false;
		this.BtnSaveII.Click += new System.EventHandler(BtnSaveII_Click);
		this.grdII.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdII).BackColor = System.Drawing.Color.White;
		grdII_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdII.DesignTimeLayout = grdII_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdII).Dock = System.Windows.Forms.DockStyle.Left;
		((System.Windows.Forms.Control)(object)this.grdII).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdII.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdII).Location = new System.Drawing.Point(350, 142);
		((System.Windows.Forms.Control)(object)this.grdII).Name = "grdII";
		((ScrollableBase)this.grdII).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdII).Size = new System.Drawing.Size(350, 499);
		((System.Windows.Forms.Control)(object)this.grdII).TabIndex = 15;
		this.grdII.TotalRow = (InheritableBoolean)1;
		this.grdII.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdII).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdII).DoubleClick += new System.EventHandler(grdII_DoubleClick);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(709, 641);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdII);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel4);
		base.Controls.Add(this.toolStrip1);
		base.KeyPreview = true;
		base.Name = "WorkStationOperation";
		this.Text = "WorkStationOperation";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(WorkStationOperation_FormClosed);
		base.Load += new System.EventHandler(WorkStationOperation_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(WorkStationOperation_KeyDown);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbworkstationname).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmboperationname).EndInit();
		this.panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdII).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
